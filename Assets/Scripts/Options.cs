using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;
    public Slider soundEffectSlider;
    public AudioSource soundEffectAudioSource;
    private const string VolumeKey = "Volume";
    private const string SoundEffectVolumeKey = "SoundEffectVolume";

    void Start()
    {
        // Retrieve the volume setting from PlayerPrefs
        float volume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        // Retrieve the sound effect volume setting from PlayerPrefs
        float soundEffectVolume = PlayerPrefs.GetFloat(SoundEffectVolumeKey, 1f);

        // Set the slider values based on the volume settings
        volumeSlider.value = volume;
        soundEffectSlider.value = soundEffectVolume;

        // Update the volume and sound effect volume immediately
        SetVolume(volume);
        SetSoundEffectVolume(soundEffectVolume);
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            // Adjust the audio source volume
            audioSource.volume = volume;
        }
        else
        {
            Debug.LogWarning("AudioManager: Audio source is null or destroyed.");
        }

        // Save the volume setting to PlayerPrefs
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        if (soundEffectAudioSource != null)
        {
            // Adjust the sound effect audio source volume
            soundEffectAudioSource.volume = volume;
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound effect audio source is null or destroyed.");
        }

        // Save the sound effect volume setting to PlayerPrefs
        PlayerPrefs.SetFloat(SoundEffectVolumeKey, volume);
    }
}
