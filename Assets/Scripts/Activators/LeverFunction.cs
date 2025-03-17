using UnityEngine;

public class LeverFunction : MonoBehaviour
{
    public bool toTheLeft;
    private Animator thisLeverAnimator;    
    private Activators thisActivator;
    private AudioSource leverAudio;
    public AudioClip leverPush;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leverAudio = GetComponent<AudioSource>();
        leverAudio.clip = leverPush;
        thisLeverAnimator= GetComponent<Animator>();
        thisActivator = GetComponent<Activators>();
    }
    
    public void UseLever()
    {
        //Mover la palanca
        leverAudio.Play();
        if (toTheLeft)
        {
            thisLeverAnimator.SetBool("toTheLeft", false);
            toTheLeft = false;
            for (int i = 0; i < thisActivator.itemsActivated.Count; i++)
            {
                thisActivator.itemsActivated[i].Activate();
            }
        }
        else
        {
            thisLeverAnimator.SetBool("toTheLeft", true);
            toTheLeft = true;
            for (int i = 0; i < thisActivator.itemsActivated.Count; i++)
            {
                thisActivator.itemsActivated[i].Deactivate();
            }
        }
    }
}
