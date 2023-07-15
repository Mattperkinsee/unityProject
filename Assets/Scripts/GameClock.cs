using UnityEngine;
using TMPro;
using System;
using UnityEngine.Video;

public class GameClock : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI daysText;
    public VideoPlayer dayBG;
    public VideoPlayer nightBG;

    private DateTime currentTime;
    private int days;

    public int GetDaysSurvived(){
        return days;
    }

    void Start()
    {
        // Set the initial time and day
        currentTime = DateTime.Parse("08:00");
        days = 1;
        
        // Start the update loop
        InvokeRepeating("UpdateTime", 0f, 1f);
    }

    void UpdateTime()
    {
        // Increment the time by 1 hour
        currentTime = currentTime.AddHours(1);

        // Check if it's a new day
        if (currentTime.Hour == 0)
        {
            days++;
        }

        // Display the updated time and days
        timeText.text = "Time: " + currentTime.ToString("HH:mm");
        daysText.text = "Days: " + days.ToString();

        // Check the time to switch between day and night backgrounds
        if (currentTime.Hour >= 8 && currentTime.Hour < 20)
        {
            // Show the day background and hide the night background
            dayBG.gameObject.SetActive(true);
            nightBG.gameObject.SetActive(false);
        }
        else
        {
            // Show the night background and hide the day background
            dayBG.gameObject.SetActive(false);
            nightBG.gameObject.SetActive(true);
        }
    }
}
