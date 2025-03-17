using UnityEngine;

public class TrapdoorFunction : MonoBehaviour
{
    public bool doesEverDamage;
    public bool canDamage;
    private Animator trapdoorAnimator;
    public bool isOpening;
    public float downwardSpeed;
    public float upwardSpeed;
    public float desfase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canDamage = false;
        trapdoorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Abrir la puerta trampa
        if (isOpening == true && (trapdoorAnimator.GetFloat("openness") < 1))
        {            
            trapdoorAnimator.SetFloat("openness",trapdoorAnimator.GetFloat("openness") + (upwardSpeed * Time.deltaTime) + desfase);
            canDamage = false;
        }
        //Cerrar la puerta trampa
        if (isOpening == false && (trapdoorAnimator.GetFloat("openness") > 0))
        {            
            trapdoorAnimator.SetFloat("openness", trapdoorAnimator.GetFloat("openness") - (downwardSpeed * Time.deltaTime) - desfase);
            //Activar damage cuando la puerta se cierra
            if (doesEverDamage == true)
            {
                canDamage = true;
            }            
        }
        else
        {
            canDamage = false;
        }
    }
    public void Close()
    {
        //Abrir trampa
        if (isOpening == true && (trapdoorAnimator.GetFloat("openness") >= 1))
        {
            isOpening = false;
        }
    }
    public void Open()
    {
        //Cerrar trampa
        if (isOpening == false)
        {
            isOpening = true;
        }
    }
}
