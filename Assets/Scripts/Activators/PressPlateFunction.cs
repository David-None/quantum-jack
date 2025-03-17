using UnityEngine;

public class PressPlateFunction : MonoBehaviour
{
    private Animator plateAnimator;
    private Activators thisActivator;
    private AudioSource plateAudio;
    public AudioClip pressedAudio, unpressedAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plateAudio = GetComponent<AudioSource>();
        plateAnimator = GetComponent<Animator>();
        thisActivator = GetComponent<Activators>();
    }

    public void PressPlate()
    {
        //Presionar la placa
        plateAudio.clip = pressedAudio;
        plateAudio.Play();
        plateAnimator.SetBool("isPressed", true);
        //Mandar la orden a todos los "activees" conectados
        for (int i = 0; i < thisActivator.itemsActivated.Count; i++)
        {
            thisActivator.itemsActivated[i].Activate();
        }
    }
    public void ReleasePlate()
    {
        //Despresionar la placa
        plateAudio.clip = unpressedAudio;
        plateAudio.Play();
        plateAnimator.SetBool("isPressed", false);
        //Mandar la orden a todos los "activees" conectados
        for (int i = 0; i < thisActivator.itemsActivated.Count; i++)
        {
            thisActivator.itemsActivated[i].Deactivate();
        }
    }
}
