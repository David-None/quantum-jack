using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumenSlider;
    public GameObject gameManager;
    public GameObject menuToOverride;
    public SaveLoad saveManager;
    public DataScript gameData;
    public TMP_Dropdown languageDropdown;
    public GameObject dropDownLabel;
    private TextMeshProUGUI dropDownLabelText;
    public GameObject startButton;
    private EventSystem eventSys;

    private void Start()
    {
        eventSys = EventSystem.current;
        dropDownLabelText = dropDownLabel.GetComponentInChildren<TextMeshProUGUI>();
        saveManager = gameManager.GetComponent<SaveLoad>();
        gameData= gameManager.GetComponent<DataScript>();        
    }
    private void OnEnable()
    {
        menuToOverride.SetActive(false);
    }
    
    public void GoBack()
    {
        gameObject.SetActive(false);
        menuToOverride.SetActive(true);
        saveManager.SaveGame();
        eventSys.SetSelectedGameObject(startButton);
    }

    public void ChangeVolume()
    {
        gameData.generalAudioVolume = volumenSlider.value;
        foreach(AudioSource audio in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            
            audio.volume= audio.gameObject.GetComponent<AudioData>().BaseVolume*volumenSlider.value;
        }
    }

    public void ChangeLanguage()
    {
        gameData.languageIndex = languageDropdown.value;
        dropDownLabelText.text = languageDropdown.options[gameData.languageIndex].text;
    }

    public void DeleteSave()
    {
        gameData.completedLevels.Clear();
        saveManager.SaveGame();
    }
}
