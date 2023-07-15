using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{

    public PlayerStats playerStats;

    public TextMeshProUGUI gameOverDays;
    public TextMeshProUGUI gameOverEnemiesKilled;
    public TextMeshProUGUI gameOverLevel;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.GetInstance();

        Debug.Log("Days:" + playerStats.GetDaysSurvived());
        Debug.Log("Level:" + playerStats.GetLevel());
        // Update the Game Over Text to display
        if (gameOverDays != null)
        {
            gameOverDays.text = "Days: " + playerStats.GetDaysSurvived();
        }

        if (gameOverEnemiesKilled != null)
        {
            gameOverEnemiesKilled.text = "Enemies Killed: " + playerStats.GetEnemiesKilled();
        }

        if (gameOverLevel != null)
        {
            gameOverLevel.text = "Level: " + playerStats.GetLevel();
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
