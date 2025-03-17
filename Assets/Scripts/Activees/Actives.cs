using UnityEngine;

public class Actives : MonoBehaviour
{
    private TrapdoorFunction thisTrapdoor;
    private CableFunction thisCable;
    private TripleDoorFunction thisTripleDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisTripleDoor = GetComponent<TripleDoorFunction>();
        thisTrapdoor = GetComponent<TrapdoorFunction>();
        thisCable = GetComponent<CableFunction>();
    }

    public void Activate()
    {
        //Activar puertas trampa
        if (thisTrapdoor != null)
        {
            thisTrapdoor.Open();
        }
        if (thisTripleDoor != null)
        {
            thisTripleDoor.SetTrap();
        }
        //Activar cables
        if (thisCable != null)
        {
            thisCable.LightOn();
        }
    }
    public void Deactivate()
    {
        //Desactivar puertas trampa
        if (thisTrapdoor != null)
        {
            thisTrapdoor.Close();
        }
        //Desactivar cables 
        if (thisCable != null)
        {
            thisCable.LightOff();
        }
    }
}
