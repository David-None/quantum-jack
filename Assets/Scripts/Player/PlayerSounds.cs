using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource jackAudio;
    public AudioClip footstepAudio, jumpAudio, deathAudio, portalAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jackAudio = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audio)
    {
        jackAudio.clip = audio;
        jackAudio.Play();
    }
}
