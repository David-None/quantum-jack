using UnityEngine;
using System.Collections.Generic;

public class DataScript : MonoBehaviour
{
    public List<string> completedLevels;
    public float generalAudioVolume;
    public int languageIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
