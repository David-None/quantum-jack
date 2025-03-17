using Unity.VisualScripting;
using UnityEngine;

public class CableFunction : MonoBehaviour
{
    public Material cableOn;
    public Material cableOff;
    public float tickingCount, tickingCountMax, tickTimeBase, tickTime, tickingTime;
    public bool isTicking;
    private bool lightsOn, lightOverride;
    private MeshRenderer thisCableRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTicking = false;
        tickingCount = 0;
        lightsOn = false;
        thisCableRenderer= GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        //Parpadeo de las luces de los cables
        if (tickingCount > 0 && isTicking && ! lightOverride)
        {
            tickingCount= tickingCount-Time.deltaTime;
            tickingTime= tickingTime-Time.deltaTime;
            if (lightsOn && (tickingTime <= 0))
            {
                thisCableRenderer.material = cableOff;
                lightsOn = false;
                tickingTime = tickTime;
            }
            else if (!lightsOn && (tickingTime <= 0))
            {
                thisCableRenderer.material = cableOn;
                lightsOn = true;
                tickingTime = tickTime;
            }
        }
        if (tickingCount <=0 && !lightOverride)
        {
            isTicking = false;
            thisCableRenderer.material = cableOff;
            lightsOn = false;
        }

        //Parpadeo mas rapido a medida que se acaba el tiempo
        if(tickingCount < (tickingCountMax / 8))
        {
            tickTime = tickTimeBase / 4;
        }
        else if (tickingCount < (tickingCountMax / 4))
        {
            tickTime = tickTimeBase / 2;
        }
        else if (tickingCount < tickingCountMax)
        {
            tickTime = tickTimeBase;
        }

    }
    public void LightOn()
    {
        if (!lightsOn)
        {
            thisCableRenderer.material = cableOn;
            lightsOn = true;
            tickingCount = 0;
            isTicking = false;
            lightOverride = true;
        }
    }
    public void LightOff()
    {
        tickingCount = tickingCountMax;
        isTicking = true;
        lightOverride = false;
    }
}
