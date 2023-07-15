using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject pauseCanvas;
    public GameObject optionsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Initially set the pause canvas to inactive
        pauseCanvas.SetActive(false);
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseCanvas.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public void PauseGame()
    {
        // Set the game canvas to inactive
        gameCanvas.SetActive(false);

        // Set the pause canvas to active
        pauseCanvas.SetActive(true);

        optionsCanvas.SetActive(false);

        // Pause any game logic or time-related processes
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        // Set the game canvas to active
        gameCanvas.SetActive(true);

        // Set the pause canvas to inactive
        pauseCanvas.SetActive(false);

        // Resume any game logic or time-related processes
        Time.timeScale = 1f;
    }

    public void ShowOptions()
    {
        // Set the game canvas to active
        gameCanvas.SetActive(false);

        // Set the pause canvas to inactive
        pauseCanvas.SetActive(false);

        optionsCanvas.SetActive(true);

         // Pause any game logic or time-related processes
        Time.timeScale = 0f;
    }
}
