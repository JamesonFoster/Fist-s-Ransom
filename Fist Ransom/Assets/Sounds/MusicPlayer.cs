using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip;   // Assign this in the Inspector
    private AudioSource audioSource;
    private float maxVolume = 1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        if (musicClip != null)
        {
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
        }
    }
    public void SetVolume(float value) 
    { 
        if (musicClip != null)
        audioSource.volume = Mathf.Clamp01(value) * maxVolume; 
    }
}