using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount;

    public PlayerStats playerStats; // Assign the PlayerStats in the inspector

    public AudioSource audioSource;  // The audio source component that will play the sound
    public AudioClip damageSound;  // The audio clip that contains the sound

    private Shake shake;
    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        playerStats = PlayerStats.GetInstance();
        healthAmount = playerStats.GetPlayerCurrentHP();
        // You can initialize the AudioSource here if it's not assigned in the inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(2);
        }
    }
    public void UpdateHPBar()
    {
        healthBar.fillAmount = playerStats.GetPlayerCurrentHP() / playerStats.GetPlayerMaxHP();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player takes:" + damage);
        healthAmount = playerStats.GetPlayerCurrentHP() - damage;
        healthBar.fillAmount = healthAmount / playerStats.GetPlayerMaxHP();
        playerStats.SetPlayerCurrentHP(healthAmount);
        playerStats.UpdateHPText();
        shake.CamShake();
        Debug.Log("Update player stats with: ");
        playerStats.UpdateDaysSurvived();
        playerStats.UpdateEnemiesKilled();
        //Player died!
        if (healthAmount <= 0)
        {
            playerStats.UpdateDaysSurvived();
            playerStats.UpdateEnemiesKilled();
            SceneManager.LoadScene("GameOver");
        }

        // Play the damage sound
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound, 0.5f);
        }
    }


    public void Heal(float healingAmount)
    {
        if (playerStats.GetPlayerCurrentHP() <= 0) return;
        healthAmount = playerStats.GetPlayerCurrentHP() + healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, playerStats.GetPlayerMaxHP());
        healthBar.fillAmount = healthAmount / playerStats.GetPlayerMaxHP();
        playerStats.Heal(healingAmount);
    }
}
