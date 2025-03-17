using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsMenu : MonoBehaviour
{
    public GameObject menuToOverride;
    public GameObject startButton;
    private EventSystem eventSys;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventSys=EventSystem.current;
    }
    
    private void OnEnable()
    {
        menuToOverride.SetActive(false);
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        menuToOverride.SetActive(true);
        eventSys.SetSelectedGameObject(startButton);
    }

    public void GoToLink(string myLink)
    {
        Application.OpenURL(myLink);
    }
}
