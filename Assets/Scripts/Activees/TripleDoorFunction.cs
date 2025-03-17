using UnityEngine;
using System.Collections.Generic;

public class TripleDoorFunction : MonoBehaviour
{
    public List<GameObject> doors;
    public List<Animator> doorAnims;
    public List<TrapdoorFunction> doorTraps;
    public float delay;
    private bool isSet;
    public float maxArmedTime;
    private float armedTimeLeft;
    public float trapSpeed;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0;
        isSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (armedTimeLeft < 0 && isSet)
        {
            DesetTrap();
        }
        if (isSet)
        {
            timer = timer+Time.deltaTime;
            armedTimeLeft = armedTimeLeft-Time.deltaTime;
            //Setear la animacion de abertura de las puertas con desfase
            doorAnims[0].SetFloat("openness", AbsoluteSin(timer*trapSpeed));
            doorAnims[1].SetFloat("openness", AbsoluteSin((timer * trapSpeed) + delay));
            doorAnims[2].SetFloat("openness", AbsoluteSin((timer * trapSpeed) + (delay * 2)));  
        }
    }
    public void SetTrap()
    {
        isSet = true;
        armedTimeLeft = maxArmedTime;
        foreach(TrapdoorFunction trap in doorTraps)
        {
            trap.canDamage = true;
        }
    }

    void DesetTrap()
    {
        foreach(TrapdoorFunction trap in doorTraps)
        {
            trap.canDamage = false;
        }
        foreach(Animator anim in doorAnims)
        {
            anim.SetFloat("openness", 0);
        }
        isSet = false;
        timer = 0;
    }
    //Formula grafica que determina la posicion de las puertas de la trampa
    float AbsoluteSin(float x)
    {
        float y;
        y= Mathf.Abs(Mathf.Sin(x));
        return y;
    }
}
