using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CorpseControl : MonoBehaviour
{
    public int corpseLimit;
    public float destroyDelayTime;
    public ObserverSpeech observerSpeech;
    public DataScript data;
    public List<GameObject> corpseList = new List<GameObject>();
    private IEnumerator wait4Destroy;
    
    public void checkCorpses(GameObject corpse)
    {
        //Introducir el ultimo cadaver generado en la lista de cadaveres
        corpseList.Add(corpse);
        //Comprobar si el numero de cadaveres supera el limite y destruir el mas viejo si se excede el limite
        if (corpseList.Count > corpseLimit) 
        {
            wait4Destroy = DelayDestroy(destroyDelayTime);
            StartCoroutine(wait4Destroy);
        }
        //Activar discurso del observador para la primera muerte del nivel
        if (corpseList.Count == 1)
        {
            if (data.languageIndex == 0)
            {
                observerSpeech.deathSpeech = observerSpeech.deathES;
            }
            else if (data.languageIndex == 1)
            {
                observerSpeech.deathSpeech = observerSpeech.deathEN;
            }
            observerSpeech.Speech(observerSpeech.deathSpeech, true);
        }
    }
    public IEnumerator DelayDestroy(float delayTime)
    {
        //Delay para destruir cadaveres, para que ocurra fuera de la vista de la camara
        yield return new WaitForSeconds(delayTime);
        Destroy(corpseList[0]);
        corpseList.Remove(corpseList[0]);
    }
}
