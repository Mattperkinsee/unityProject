using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects Instance;

    public AudioClip levelUpSound;
    public AudioClip healSound;
    // Add more AudioClip fields for other sound effects

    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure only one instance of SoundEffects exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLevelUpSound()
    {
        audioSource.PlayOneShot(levelUpSound);
    }

    public void PlayHealSound()
    {
        audioSource.PlayOneShot(healSound);
    }

    // Add more methods for other sound effects
}
