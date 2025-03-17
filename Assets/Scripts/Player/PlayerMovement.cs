using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset inputGestor;
    private InputAction moveInput, jumpInput, runInput, activateInput, pauseInput;
    private Rigidbody playerRB;
    private CapsuleCollider playerCollider;
    public float playerBaseSpeed, runMultiplier, jumpForce;
    private float playerSpeed;
    public bool onGround, nearEdge,nearEdgeL,nearEdgeR, onEdge;
    private Transform edgeTransform;
    public float edgeLDesfasePrevioX, edgeLDesfasePrevioY, edgeLDesfasePostX, edgeLDesfasePostY;
    public float edgeRDesfasePrevioX, edgeRDesfasePrevioY, edgeRDesfasePostX, edgeRDesfasePostY;
    private Animator playerAnim;
    public float idleMaxCountdown, crawlMaxCountdown;
    private float idleCountdown, crawlCountdown;
    public Transform spriteTransform;
    public bool edgeAnimEnd;
    public CameraMovement myCamera;
    private DoorFunction door;
    private bool canFinishLevel;
    public Activators currentActivator;
    public bool canActivate;
    public float maxGroundDistance;
    public bool groundBelow;
    public bool onPlate;
    public bool onPause;
    public GameObject pauseMenu;
    private AudioSource jackAudio;
    private PlayerSounds jackSounds;
    private bool endingLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteTransform.gameObject.SetActive(true);
        jackSounds = GetComponent<PlayerSounds>();
        onPause = false;
        playerAnim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        moveInput = inputGestor.FindAction("Move");
        runInput = inputGestor.FindAction("Sprint");
        activateInput = inputGestor.FindAction("Interact");
        pauseInput = inputGestor.FindAction("Pause");
        onEdge = false;
        idleCountdown = idleMaxCountdown;
        crawlCountdown = crawlMaxCountdown;
        edgeAnimEnd = true;
        canFinishLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseInput.WasPressedThisFrame())
        {
            onPause = true;
            pauseMenu.SetActive(true);
        }
        if (!onPause && !endingLevel)
        {
            //Activar y desactivar correr
            if (runInput.IsPressed())
            {
                playerSpeed = playerBaseSpeed * runMultiplier;
            }
            else
            {
                playerSpeed = playerBaseSpeed;
            }
            //Movimiento lateral
            if ((moveInput.ReadValue<Vector2>().x != 0) && (onEdge == false))
            {
                transform.Translate(moveInput.ReadValue<Vector2>().x * playerSpeed * Time.deltaTime, 0, 0);
                playerAnim.SetBool("Walking", true);
                if (moveInput.ReadValue<Vector2>().x < 0)
                {
                    spriteTransform.localScale = new Vector3(-11, 11, 10);
                }
                else if (moveInput.ReadValue<Vector2>().x > 0)
                {
                    spriteTransform.localScale = new Vector3(11, 11, 10);
                }
            }
            else
            {
                playerAnim.SetBool("Walking", false);
            }

            //Idle activation and countdown
            idleCountdown = idleCountdown - Time.deltaTime;
            if ((idleCountdown < 0) && (moveInput.ReadValue<Vector2>().x == 0))
            {
                playerAnim.SetFloat("IdleTreeRandom1", UnityEngine.Random.Range(-1, 1));
                playerAnim.SetFloat("IdleTreeRandom2", UnityEngine.Random.Range(-1, 1));
                playerAnim.SetTrigger("IdleTrigger");
                idleCountdown = idleMaxCountdown;
            }

            //Salto
            if ((moveInput.ReadValue<Vector2>().y > 0) && onGround && !onEdge)
            {
                jackSounds.PlayAudio(jackSounds.jumpAudio);
                playerRB.AddForce(0, jumpForce, 0);
                
                onGround = false;
                playerAnim.SetBool("OnGround", false);
                playerAnim.SetTrigger("Jump");
                playerAnim.SetBool("ReachGround", false);
            }

            //Agacharse
            if ((moveInput.ReadValue<Vector2>().y < 0) && onGround && !onEdge)
            {
                playerCollider.height = 1f;
                playerCollider.center = new Vector3(0f, 0.5f, 0f);
            }
            else
            {
                playerCollider.height = 2f;
                playerCollider.center = new Vector3(0f, 1f, 0f);
            }

            //Agarrar borde
            if (onGround == false && nearEdge == true && !onEdge)
            {
                if (nearEdgeL && spriteTransform.localScale.x > 0f)
                {
                    gameObject.transform.position = new Vector3(edgeTransform.position.x + edgeLDesfasePrevioX, edgeTransform.position.y + edgeLDesfasePrevioY, gameObject.transform.position.z);
                    GrabEdge();
                }
                else if (nearEdgeR && spriteTransform.localScale.x < 0f)
                {
                    gameObject.transform.position = new Vector3(edgeTransform.position.x + edgeRDesfasePrevioX, edgeTransform.position.y + edgeRDesfasePrevioY, gameObject.transform.position.z);
                    GrabEdge();
                }
            }

            if (onEdge)
            {
                crawlCountdown = crawlCountdown - Time.deltaTime;
            }

            //Subir borde
            if ((moveInput.ReadValue<Vector2>().y > 0) && onEdge && (crawlCountdown < 0))
            {
                playerAnim.SetBool("CornerCrawl", true);
            }

            //Bajar borde
            if (moveInput.ReadValue<Vector2>().y < 0 && onEdge && (crawlCountdown < 0))
            {
                onEdge = false;
                nearEdge = false;
                playerRB.isKinematic = false;
                gameObject.transform.Translate(0, -0.1f, 0);
                playerAnim.SetBool("EdgeDrop",true);
            }

            //Activar objetos
            if (canActivate && activateInput.WasPressedThisFrame())
            {

                playerAnim.SetBool("Push", true);

                if (currentActivator != null)
                {
                    currentActivator.Activate();
                }
            }

            //Superar nivel
            if (canFinishLevel && activateInput.WasPressedThisFrame())
            {
                playerAnim.SetBool("EndLevel", true);
            }
        }
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //Detectar el suelo
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Pressplate")
        {
            onGround = true;
            playerAnim.SetBool("OnGround", true);
            playerAnim.SetBool("EdgeDrop", true);
        }
        //Detectar placa de presion
        if (other.gameObject.tag == "Pressplate")
        {
            onPlate = true;
            if (other.gameObject.GetComponent<Activators>() != null)
            {
                other.gameObject.GetComponent<Activators>().Activate();
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Detectar un borde
        if (other.gameObject.tag == "LowEdge")
        {
            if (other.gameObject.name == "LowEdgeL")
            {
                nearEdgeL = true;
            }
            else if (other.gameObject.name == "LowEdgeR")
            {
                nearEdgeR = true;
            }
            nearEdge = true;
            edgeTransform = other.gameObject.transform;
        }
        //Activar animacion de aterrizaje
        if (other.gameObject.tag == "LandingThreshold")
        {
            playerAnim.SetBool("ReachGround",true);
        }
        //Detectar puerta
        if (other.gameObject.tag == "Door")
        {
            //canFinishLevel = true;
            door = other.GetComponent<DoorFunction>();
        }
        //Permitir pasar de nivel
        if (other.gameObject.tag == "Door" && door.isOpen)
        {
            canFinishLevel = true;
        }
        //Detectar "Activadores"
        if ((other.gameObject.layer == 3) && (other.GetComponent<Activators>()!=null))
        {
            currentActivator = other.GetComponent<Activators>();
            canActivate = true;
            if (currentActivator.useIcon != null)
            {
                currentActivator.ShowUseIcon();
            }
            
        }    }
    private void OnTriggerExit(Collider other)
    {
        //Salir del borde de agarre
        if (other.gameObject.tag == "LowEdge")
        {
            nearEdge = false;
            nearEdgeL = false;
            nearEdgeR = false;
            playerAnim.SetBool("EdgeDrop", false);
        }
        if (other.gameObject.tag == "MainCamera")
        {
            myCamera.followPlayer = true;
        }
        if (other.gameObject.tag == "Door")
        {
            canFinishLevel = false;
        }
        if ((other.gameObject.layer == 3) && (other.GetComponent<Activators>() != null))
        {
            currentActivator = other.GetComponent<Activators>();
            canActivate = false;
            if (currentActivator.useIcon != null)
            {
                currentActivator.HideUseIcon();
            }              
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Pressplate")
        {
            onPlate = false;
            if (other.gameObject.GetComponent<Activators>() != null)
            {
                other.gameObject.GetComponent<Activators>().Deactivate();
            }
        }
        if ((other.gameObject.tag == "Ground") && !onPlate)
        {
            onGround = false;
            playerAnim.SetBool("OnGround", false);
            playerAnim.SetBool("ReachGround", false);
        }
    }

    private void EndCrawl()
    {
        //Colocar al jugador en la posicion adecuada tras terminar de subir un borde
        if (spriteTransform.localScale.x > 0f)
        {
            transform.position = new Vector3(edgeTransform.position.x + edgeLDesfasePostX, edgeTransform.position.y + edgeLDesfasePostY, gameObject.transform.position.z);
        }
        if (spriteTransform.localScale.x < 0f)
        {
            transform.position = new Vector3(edgeTransform.position.x + edgeRDesfasePostX, edgeTransform.position.y + edgeRDesfasePostY, gameObject.transform.position.z);
        }
        
        playerAnim.SetBool("CornerCrawl", false);
        playerRB.isKinematic = false;
        onEdge = false;        
        onGround = true;
        playerAnim.SetBool("OnGround", true);
        playerAnim.SetBool("EdgeDrop", false);
    }
    private void EndHang()
    {
        playerAnim.SetBool("CornerHang", false);
        playerAnim.SetBool("EdgeDrop", false);
        edgeAnimEnd = true;
    }
    private void GrabEdge()
    {
        playerAnim.SetBool("CornerHang", true);
        playerRB.isKinematic = true;
        onEdge = true;
        edgeAnimEnd = false;
        crawlCountdown = crawlMaxCountdown;
    }

    public void TakeControlAtExit()
    {
        endingLevel = true;
    }
    public void DoExitLevel()
    {
        playerAnim.SetBool("EndLevel", false);
        spriteTransform.gameObject.SetActive(false);
        door.FinishingClose();
    }

    public void GiveControlBack()
    {
        endingLevel = false;
    }
}
