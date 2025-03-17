using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel, creditsPanel;
    public GameObject optionsBackButton, creditsBackButton;
    private EventSystem eventSys;
    public GameObject foreground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreground.SetActive(true);
        eventSys = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Intro");
    }
    public void OptionButton()
    {
        optionsPanel.SetActive(true);
        eventSys.SetSelectedGameObject(optionsBackButton);
    }
    public void CreditsButton()
    {
        creditsPanel.SetActive(true);
        eventSys.SetSelectedGameObject(creditsBackButton);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void DeActForeground()
    {
        foreground.SetActive(false);
    }
}
