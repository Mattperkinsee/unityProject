using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioSource soundEffectSource;

    private const string VolumeKey = "Volume";
    private const string SoundEffectVolumeKey = "SoundEffectVolume";

    private void Awake()
    {
        // Make sure there's only one instance of the AudioManager
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load the saved volume from PlayerPrefs and set the audio source volume
        if (PlayerPrefs.HasKey(VolumeKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey);
            SetVolume(savedVolume);
        }

        // Load the saved sound effect volume from PlayerPrefs and set the sound effect source volume
        if (PlayerPrefs.HasKey(SoundEffectVolumeKey))
        {
            float savedSoundEffectVolume = PlayerPrefs.GetFloat(SoundEffectVolumeKey);
            SetSoundEffectVolume(savedSoundEffectVolume);
        }
    }

    public void SetVolume(float volume)
    {
        // Adjust the audio source volume
        audioSource.volume = volume;

        // Save the volume setting to PlayerPrefs
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        // Adjust the sound effect source volume
        soundEffectSource.volume = volume;

        // Save the sound effect volume setting to PlayerPrefs
        PlayerPrefs.SetFloat(SoundEffectVolumeKey, volume);
    }
}
