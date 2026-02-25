using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip;   // Assign this in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }
    public void SetVolume(float value) 
    { 
        audioSource.volume = Mathf.Clamp01(value) * maxVolume; 
    }
}