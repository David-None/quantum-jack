using System.Runtime.CompilerServices;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObserverSpeech : MonoBehaviour
{
    public Image speechBubbleVisual;
    public string[] levelEntranceES, levelEntranceEN, deathES, deathEN;
    public string[] levelEntranceSpeech, deathSpeech;
    public DataScript data;
    public string currentText;
    public float charWaitTime, lineWaitTime;
    public TextMeshProUGUI speechText;
    private IEnumerator wait4Speech;
    private AudioSource observerAudio;
    public AudioClip observerAudioClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        observerAudio = GetComponent<AudioSource>();
        observerAudio.clip = observerAudioClip;
        //Discurso de inicio de nivel
        if (data.languageIndex == 0 )
        {
            levelEntranceSpeech = levelEntranceES;
        }
        if (data.languageIndex == 1 )
        {
            levelEntranceSpeech=levelEntranceEN;
        }        
        Speech(levelEntranceSpeech, false);
    }

    public void Speech(string[] speechString, bool interrupt) 
    {
        //Activar discurso
        if (interrupt)
        {
            StopCoroutine(wait4Speech);
        }
        speechBubbleVisual.enabled = true;
        speechText.enabled = true;
        wait4Speech = writeLoop(speechString, charWaitTime, lineWaitTime);
        StartCoroutine(wait4Speech);                
    }

    public IEnumerator writeLoop(string[] speechLines, float charTime, float lineTime)
    {
        //Loop de escritura de caracteres del discurso
        for (int i = 0; i < speechLines.Length; i++)
        {
            char[] charArray = speechLines[i].ToCharArray();
            currentText = "";
            for (int j = 0; j < charArray.Length; j++)
            {
                currentText += charArray[j];
                observerAudio.Play();
                speechText.text = currentText;
                yield return new WaitForSeconds(charTime);
            }
            yield return new WaitForSeconds(lineTime);
        }
        speechBubbleVisual.enabled = false;
        speechText.enabled = false;
    }
}
