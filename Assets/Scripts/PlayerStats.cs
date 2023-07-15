using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Declare the static instance variable
    private static PlayerStats instance;
    // Key for PlayerPrefs
    private const string PlayerPrefsKey = "PlayerStatsData";

    //initialize values
    public int xp = 0; // Player's XP
    public int level = 1; // Player's Level
    public int xpToNextLevel = 10; // XP required to reach the next level
    public float playerCurrentHP = 10f; //Starting Player's HP
    public float playerMaxHP = 10f; //Starting Player's Max HP
    public int playerCurrentXP = 0; //Starting Player's XP
    public int playerPrevXP = 0;
    public int daysSurvived = 0;
    public int enemiesKilled = 0;

    // Data class to store the player stats
    [System.Serializable]
    public class PlayerStatsData
    {
        public int xp;
        public int level;
        public int xpToNextLevel;
        public float playerCurrentHP;
        public float playerMaxHP;
        public int playerCurrentXP;
        public int playerPrevXP;
        public int daysSurvived;
        public int enemiesKilled;
    }


    //external components
    public HealthManager healthManager; // Assign the HealthManager in the inspector
    public GameClock gameClock; // Assign the HealthManager in the inspector
    public EnemyCounter enemyCounter; // Assign the HealthManager in the inspector
    public Image XPBar;


    //text components
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI hpText;

    //audio components
    public AudioSource audioSource;  // The audio source component that will play the sound
    public AudioClip levelUpSound;  // The audio clip that contains the sound
    public AudioClip healSound;  // The audio clip that contains the sound
    public void IncrementXP()
    {
        xp++; // Increment the XP by 1
        // Update the TextMeshPro component with the new XP value
        xpText.text = "XP: " + GetXP().ToString();
        if (xp >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateXPBar();

        // You can perform additional actions or logic here based on the XP increase
        Debug.Log("Player XP increased! Current XP: " + xp);
    }

    public void UpdateXPBar()
    {
        int currentXP = GetXP();
        int nextLevelXP = GetXPToNextLevel();
        int prevXP = GetPrevXP();
        int xpDiff = GetXPDiff();

        // if (currentXP >= nextLevelXP)
        // {
        //     currentXP -= nextLevelXP;
        //     nextLevelXP = GetXPToNextLevel();
        // }

        float fillAmount = ((float)currentXP - (float)prevXP) / ((float)nextLevelXP - (float)prevXP);
        XPBar.fillAmount = fillAmount;
    }

    public void UpdateHPText()
    {
        // Update the TextMeshPro component with the new HP values
        hpText.text = "HP: " + GetPlayerCurrentHP().ToString() + "/" + GetPlayerMaxHP().ToString();
    }

    private void LevelUp()
    {
        level++; // Increment the level
        playerPrevXP = GetXP();
        xpToNextLevel += 10; // Increase the XP required for the next level

        playerMaxHP += 10;
        UpdateHPText();
        healthManager.UpdateHPBar();


        // Play the level up sound
        if (levelUpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(levelUpSound);
        }
        // Update the playerLevelText to display the updated level
        if (playerLevelText != null)
        {
            playerLevelText.text = "Level: " + level;
        }

        // You can perform additional actions or logic here when the player levels up
        Debug.Log("Player leveled up! Current Level: " + level);

    }

    public void Heal(float healingAmount)
    {
        if (Mathf.Approximately(playerCurrentHP + healingAmount, playerMaxHP))
        {
            playerCurrentHP = playerMaxHP;
        }
        else if (playerCurrentHP + healingAmount < playerMaxHP)
        {

            // Play the level up sound
            if (healSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(healSound);
            }
            playerCurrentHP += healingAmount;
        }

        UpdateHPText();

    }

    public int GetXP()
    {
        return xp;
    }

    public int GetPrevXP()
    {
        return playerPrevXP;
    }

    public int GetXPDiff()
    {
        return GetXPToNextLevel() - GetXP();
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetPlayerMaxHP()
    {
        return playerMaxHP;
    }

    public float GetPlayerCurrentHP()
    {
        return playerCurrentHP;
    }

    public int GetXPToNextLevel()
    {
        return xpToNextLevel;
    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    public int GetDaysSurvived()
    {
        return daysSurvived;
    }

    public void UpdateDaysSurvived()
    {
        daysSurvived = gameClock.GetDaysSurvived();
    }

    public void UpdateEnemiesKilled()
    {
        enemiesKilled = enemyCounter.GetEnemiesKilled();
    }



    // Start is called before the first frame update
    void Start()
    {

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

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set this instance as the singleton instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists and it's not this one, destroy this duplicate
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        LoadPlayerStats();
    }

    private void OnDisable()
    {
        SavePlayerStats();
    }

    private void LoadPlayerStats()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string jsonData = PlayerPrefs.GetString(PlayerPrefsKey);
            PlayerStatsData statsData = JsonUtility.FromJson<PlayerStatsData>(jsonData);

            // Load the saved player stats
            xp = statsData.xp;
            level = statsData.level;
            xpToNextLevel = statsData.xpToNextLevel;
            playerCurrentHP = statsData.playerCurrentHP;
            playerMaxHP = statsData.playerMaxHP;
            playerCurrentXP = statsData.playerCurrentXP;
            playerPrevXP = statsData.playerPrevXP;
            daysSurvived = statsData.daysSurvived;
            enemiesKilled = statsData.enemiesKilled;

            Debug.Log("Player stats loaded.");
        }
    }

    private void SavePlayerStats()
    {
        // Create a data object to store the player stats
        PlayerStatsData statsData = new PlayerStatsData
        {
            xp = xp,
            level = level,
            xpToNextLevel = xpToNextLevel,
            playerCurrentHP = playerCurrentHP,
            playerMaxHP = playerMaxHP,
            playerCurrentXP = playerCurrentXP,
            playerPrevXP = playerPrevXP,
            daysSurvived = daysSurvived,
            enemiesKilled = enemiesKilled
        };

        // Convert the data object to JSON
        string jsonData = JsonUtility.ToJson(statsData);

        // Save the JSON string to PlayerPrefs
        PlayerPrefs.SetString(PlayerPrefsKey, jsonData);

        Debug.Log("Player stats saved.");
    }

    // Create a public static method to access the singleton instance
    public static PlayerStats GetInstance()
    {
        return instance;
    }
}


