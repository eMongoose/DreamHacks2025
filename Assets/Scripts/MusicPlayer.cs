using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;  // Assign your AudioSource in the Inspector

    private static MusicPlayer instance;

    void Awake()
    {
        // Ensure only one instance exists (Singleton pattern)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep music playing across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }
    }

    void Start()
    {
        // Play music if not already playing
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    // Optional: Method to stop the music
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}