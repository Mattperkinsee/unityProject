using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tome : MonoBehaviour
{

    public PlayerStats playerStats; // Reference to the playerStats script


    public AudioSource audioSource;  // The audio source component that will play the sound
    public AudioClip tomeClickSound;  // The audio clip that contains the sound
    public void OnClickIncrementXP()
    {
        // Call a method or increment a variable in the PlayerXP script to increase the XP
        playerStats.IncrementXP(1);



        // Play the tome click sound
        if (tomeClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(tomeClickSound);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.GetInstance();
        // You can initialize the AudioSource here if it's not assigned in the inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
