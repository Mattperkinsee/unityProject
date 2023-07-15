using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public PlayerStats playerStats; // Assign the PlayerStats in the inspector
    
    public void PlayGame()
    {
        // Reset the player stats
        playerStats.ResetPlayerStats();

        // Load the Game scene
        SceneManager.LoadScene("Game");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
