using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float cameraSpeed;
    public GameObject player;
    public Vector3 targetRotationPlayer;
    public Vector2 targetTranslationPlayer;
    public bool followPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        followPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) 
        {
            //Mirar en la direccion del jugador
            targetRotationPlayer = (player.transform.position - cameraTransform.position).normalized;
            cameraTransform.transform.rotation = Quaternion.LookRotation(targetRotationPlayer);

            //Si el jugador se aleja del foco de la camara, seguirlo
            if (followPlayer)
            {
                targetTranslationPlayer = (new Vector3(player.transform.position.x, player.transform.position.y+1, player.transform.position.z) - transform.position).normalized;
                transform.Translate(targetTranslationPlayer.x * cameraSpeed * Time.deltaTime, targetTranslationPlayer.y * cameraSpeed * Time.deltaTime, 0);
            }
            if ((transform.position.x == player.transform.position.x) && (transform.position.y == player.transform.position.y+1))
            {
                followPlayer = false;
            }
        }
        
    }

    private void OnColliderExit(Collider other)
    {
        //Detectar que el jugador se aleja del foco
        if (other.gameObject.tag == "Player")
        {
            followPlayer = true;
        }
    }
}
