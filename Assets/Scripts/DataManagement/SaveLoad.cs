using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class SaveLoad : MonoBehaviour
{
    public DataScript data;
    public string filePath;
    private DataScript thisData;
    private SaveLoad thisSave;
    private SaveLoad duplicate;
    private DoorFunction[] doors;
    private OptionsMenu optionsMenu;
    private SceneTextScroll textScene;
    private ObserverSpeech oSpeech;
    private CorpseControl corpseControl;

    private void Awake()
    {
        filePath = Application.streamingAssetsPath + "/playerData.json";
        data= GetComponent<DataScript>();
        LoadGame();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisSave = GetComponent<SaveLoad>();
        thisData= GetComponent<DataScript>();

        //Encontrar duplicados
        foreach (SaveLoad saveScript in FindObjectsByType<SaveLoad>(FindObjectsSortMode.None))
        {
            if (saveScript != thisSave)
            {
                duplicate = saveScript;                
            }
        } 
        
        //Asignar las referencias en cada escena y destruir el duplicado
        if (duplicate != null)
        {
            //Puertas
            doors = FindObjectsByType<DoorFunction>(FindObjectsSortMode.None);
            if (doors != null)
            {
                foreach (DoorFunction door in doors)
                {
                    door.gameManager = this.gameObject;
                    door.gameData = thisData;
                }
            }
            

            //Menu de opciones
            optionsMenu = FindFirstObjectByType<OptionsMenu>();
            
            if (optionsMenu != null)
            {
                Debug.Log(optionsMenu.name);
                optionsMenu.gameManager = this.gameObject;
                optionsMenu.gameData = thisData;
                optionsMenu.saveManager = thisSave;
            }

            //Escenas de texto
            textScene = FindFirstObjectByType<SceneTextScroll>();
            if (textScene != null)
            {
                textScene.data = thisData;
            }

            //Discursos del observador
            oSpeech = FindFirstObjectByType<ObserverSpeech>();
            corpseControl = FindFirstObjectByType<CorpseControl>();
            if (oSpeech != null)
            {
                oSpeech.data = thisData;
            }
            if (corpseControl != null)
            {
                corpseControl.data = thisData;
            }            

            //Destruir doble
            Destroy(duplicate.gameObject);
        }

        data = GetComponent<DataScript>();
        filePath = Application.streamingAssetsPath+"/playerData.json";

        //Cargar los datos en cada escena para no sobreescribirlos
        LoadGame();
        if (SceneManager.GetActiveScene().name == "LevelHub")
        {
            doors = FindObjectsByType<DoorFunction>(FindObjectsSortMode.None);
            foreach (DoorFunction door in doors)
            {
                if (data.completedLevels.Contains(door.toSceneName))
                {
                    door.isOpen = true;
                    door.GetComponent<Animator>().SetBool("isOpen", true);
                }
            }
        }

        //Setear todos los audios de la escena
        foreach (AudioSource audio in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            audio.volume = audio.gameObject.GetComponent<AudioData>().BaseVolume * thisData.generalAudioVolume;
        }
    }

    public void SaveGame()
    {
        string json =JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(filePath, json);
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(filePath))
        {
            string jsonLeido = System.IO.File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(jsonLeido, data);
        }
    }
}
