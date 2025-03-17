using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DoorFunction : MonoBehaviour
{
    public bool isOpen, isGoodPortal, isHubPortal;
    public string toSceneName;
    public Animator doorAnimator;
    public GameObject gameManager;
    public DataScript gameData;
    private SaveLoad saveLoad;
    private AudioSource doorAudio;

    //private Coroutine endRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //endRoutine = TheEnd(30.0f);
        doorAudio = GetComponent<AudioSource>();
        gameData = gameManager.GetComponent<DataScript>();
        saveLoad = gameManager.GetComponent<SaveLoad>();
        doorAnimator = GetComponent<Animator>();

        //Condiciones para abrir los portales del hub
        if (isHubPortal && gameData.completedLevels.Contains(toSceneName))
        {
            isOpen = true;
        }
        if (isOpen)
        {
            doorAnimator.SetBool("isOpen", true);
        }
    }
    private IEnumerator TheEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EndLevel();
    }

    //Cerrar al superar el nivel
    public void FinishingClose()
    {
        doorAnimator.SetBool("isOpen", false);
        doorAnimator.SetBool("EndLevel", true);
        StartCoroutine(TheEnd(4.3f));
    }

    //Guardar y terminar el nivel tras cerrar la puerta
    public void EndLevel()
    {
        /*
        if (!gameData.completedLevels.Contains(toSceneName))
        {
            gameData.completedLevels.Add(toSceneName);
        }           
        saveLoad.SaveGame();        
        */
        SceneManager.LoadScene(toSceneName);
    }

    public void DoorAudio(AudioClip audio)
    {
        doorAudio.clip = audio;
        doorAudio.Play();
    }
}
