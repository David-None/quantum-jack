using NUnit.Framework.Constraints;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerDeaths : MonoBehaviour
{
    public CorpseControl corpseControl;
    public GameObject goodPortal, badPortal, jackPrefab, bloodSplosion, spikeDeathSprite, trapDeathSprite;
    public float wait4DeathStart, wait4Death;
    public TheObserver[] observers;
    public bool waitCounter;
    public SpriteRenderer jackSprite;
    private GameObject newJack;
    private Collider jackCollider;
    private Vector3 spawnPos;
    private Quaternion spawnQuat;
    private DoorFunction goodPortalF, badPortalF;
    private Animator goodPortalAnim, badPortalAnim;
    private PlayerMovement thisPlayer;
    private CameraMovement theCamera;
    private GameObject deathSprite, newCorpse;
    public PauseMenu pauseMenu;
    private PlayerSounds jackSounds;
    private GameObject deathSpriteParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jackSounds = GetComponent<PlayerSounds>();
        jackCollider = GetComponent<Collider>();
        spawnPos = transform.position;
        spawnQuat = transform.rotation;
        goodPortalAnim = goodPortal.GetComponent<Animator>();
        badPortalAnim = badPortal.GetComponent<Animator>();
        goodPortalF = goodPortal.GetComponent<DoorFunction>();
        badPortalF = badPortal.GetComponent<DoorFunction>();
        thisPlayer = GetComponent<PlayerMovement>();
        theCamera = thisPlayer.myCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitCounter)
        {
            //Cuentra atras y respawn
            wait4Death=wait4Death - Time.deltaTime;
            if (wait4Death < 0)
            {
                waitCounter = false;
                JackSpawn();
            }
        }        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            //Diferencias según la trampa
            if (other.gameObject.tag == "Spiketrap")
            {
                //Trampas de pinchos
                deathSprite = spikeDeathSprite;
                KillJack();
            }
            if (other.gameObject.tag == "Trapdoor" && other.gameObject.GetComponentInParent<TrapdoorFunction>().canDamage)
            {
                //Puertas trampa
                deathSprite = trapDeathSprite;
                deathSpriteParent = other.gameObject;
                KillJack();
            }            
        }
    }
    private void JackSpawn()
    {
        //Respawn
        jackSprite.enabled = true;
        jackCollider.enabled=true;
        newJack = Instantiate(jackPrefab, spawnPos, spawnQuat);
        theCamera.player = newJack;
        pauseMenu.player = newJack;
        for (int i = 0; i < observers.Length ; i++)
        {
            observers[i].player = newJack;
            observers[i].GetLookAt();
        }        
        Destroy(gameObject);
    }

    private void KillJack()
    {        
        jackSounds.PlayAudio(jackSounds.deathAudio);
        //Abrir y cerrar las puertas correspondientes al morir
        if (goodPortalF.isOpen)
        {
            goodPortalF.isOpen = false;
            goodPortalAnim.SetBool("isOpen", false);
        }
        if (badPortalF.isOpen == false)
        {
            badPortalF.isOpen = true;
            badPortalAnim.SetBool("isOpen", true);
        }
        //Efectos de muerte
        Instantiate(bloodSplosion, transform.position, transform.rotation);
        newCorpse = Instantiate(deathSprite, transform.position, transform.rotation);        
        if (jackSprite.transform.localScale.x < 0)
        {
            newCorpse.transform.localScale = new Vector3(-newCorpse.transform.localScale.x, newCorpse.transform.localScale.y, newCorpse.transform.localScale.z);
        }
        if (deathSpriteParent != null)
        {
            newCorpse.transform.parent= deathSpriteParent.transform;
            newCorpse.transform.position = new Vector3(newCorpse.transform.position.x, newCorpse.transform.position.y + 1.7f, newCorpse.transform.position.z);
        }
        //Desabilitar camara, control y llamar a corpseControl
        jackSprite.enabled = false;
        theCamera.player = null;
        jackCollider.enabled = false;
        corpseControl.checkCorpses(newCorpse);
        //Crear cuenta atras para respawn
        wait4Death = wait4DeathStart;
        waitCounter = true;
    }
}
