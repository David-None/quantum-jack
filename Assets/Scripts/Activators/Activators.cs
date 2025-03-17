using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Activators : MonoBehaviour
{
    public List<Actives> itemsActivated;
    public GameObject[] objectsActivated;
    public GameObject useIcon;
    private LeverFunction thisLever;
    private PressPlateFunction thisPressplate;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Llamar a todos los "activees" conectados a este activador
        for (int i = 0; i< objectsActivated.Length; i++)
        {
            if (objectsActivated[i].GetComponent<Actives>() != null)
            {
                
                itemsActivated.Add(objectsActivated[i].GetComponent<Actives>());
            }
            else
            {
                
                foreach (Transform act in objectsActivated[i].GetComponentInChildren<Transform>())
                {
                    itemsActivated.Add(act.gameObject.GetComponent<Actives>());
                }
            }
        }
        thisLever = GetComponent<LeverFunction>();
        thisPressplate = GetComponent<PressPlateFunction>();
    }
    //Mostrar/Ocultar icono de objeto usable
    public void ShowUseIcon()
    {
        useIcon.SetActive(true);
    }
    public void HideUseIcon() 
    {
        useIcon.SetActive(false);
    }
    public void Activate()
    {
        //Activar palancas
        if (thisLever != null)
        {
            thisLever.UseLever();
        }
        //Activar placas de presion
        if (thisPressplate != null)
        {
            thisPressplate.PressPlate();
        }
    }
    public void Deactivate()
    {
        //Desactivar placas
        if (thisPressplate != null)
        {
            thisPressplate.ReleasePlate();
        }
    }
}
