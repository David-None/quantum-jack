using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTextScroll : MonoBehaviour
{
    public string speechES, speechEN;
    private string speechToWrite;
    public float charWaitTime;
    private string currentText;

    public TextMeshProUGUI speechText;
    private IEnumerator wait4Speech;

    private AudioSource scrollAudio;
    public AudioClip scrollAudioClip;

    public string nextScene;
    public DataScript data;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {             
        scrollAudio = GetComponent<AudioSource>();
        scrollAudio.clip = scrollAudioClip;

        //Iniciar el texto al comenzar la escena
        data.gameObject.GetComponent<SaveLoad>().LoadGame();
        if (data.languageIndex == 1)
        {
            speechToWrite = speechEN;
        }
        else if (data.languageIndex == 0)
        {
            speechToWrite = speechES;
        }
        SpeechScroll(speechToWrite);
    }    

    //Llamada a la corutina principal
    void SpeechScroll(string speech)
    {
        wait4Speech = scrollWriteLoop(speech, charWaitTime);
        StartCoroutine(wait4Speech);
    }

    //Corutina de escritura de texto
    public IEnumerator scrollWriteLoop(string speech, float charTime)
    {

        char[] charArray = speech.ToCharArray();
        currentText = "";
        for (int j = 0; j < charArray.Length; j++)
        {
            currentText += charArray[j];
            scrollAudio.Play();
            speechText.text = currentText;
            yield return new WaitForSeconds(charTime);
        }
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(nextScene);
    }

    public void SkipButton()
    {
        SceneManager.LoadScene(nextScene);
    }
}
