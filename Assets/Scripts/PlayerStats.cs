using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    // Declare the static instance variable
    private static PlayerStats instance;
    // Key for PlayerPrefs
    private const string PlayerPrefsKey = "PlayerStatsData";

    //initialize values
    public int xp; // Player's XP
    public int level; // Player's Level
    public int xpToNextLevel; // XP required to reach the next level
    public float playerCurrentHP; //Starting Player's HP
    public float playerMaxHP; //Starting Player's Max HP
    public int playerCurrentXP; //Starting Player's XP
    public int playerPrevXP;
    public int daysSurvived;
    public int enemiesKilled;

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
    public HealthManager healthManager;
    public GameClock gameClock;
    public EnemyCounter enemyCounter;
    public Image XPBar;

    //text components
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI hpText;

    //audio components
    private AudioSource audioSource;
    public AudioClip levelUpSound;
    public AudioClip healSound;


    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SavePlayerStats();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Game")
        {
            InitializeGameScene();
        }
        else if (scene.name == "Main")
        {
            InitializeMainMenu();
        }
    }

    private void InitializeGameScene()
    {
        gameClock = FindObjectOfType<GameClock>();
        enemyCounter = FindObjectOfType<EnemyCounter>();
        healthManager = FindObjectOfType<HealthManager>();

        playerLevelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        xpText = GameObject.Find("XPText").GetComponent<TextMeshProUGUI>();
        hpText = GameObject.Find("HPText").GetComponent<TextMeshProUGUI>();

        XPBar = GameObject.Find("XPBarFill").GetComponent<Image>();
        if (XPBar == null)
        {
            Debug.LogError("XPBarFill not found in the scene!");
        }



        LoadPlayerStats();
    }

    private void InitializeMainMenu()
    {
        // Perform initialization specific to the Main scene
        // ...
    }

    public void IncrementXP()
    {
        xp++;
        xpText.text = "XP: " + GetXP().ToString();

        if (xp >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateXPBar();

        Debug.Log("Player XP increased! Current XP: " + xp);
    }

    public void UpdateXPBar()
    {
        int currentXP = GetXP();
        int nextLevelXP = GetXPToNextLevel();
        int prevXP = GetPrevXP();

        float fillAmount = ((float)currentXP - (float)prevXP) / ((float)nextLevelXP - (float)prevXP);
        XPBar.fillAmount = fillAmount;
    }

    public void UpdateHPText()
    {
        hpText.text = "HP: " + GetPlayerCurrentHP().ToString() + "/" + GetPlayerMaxHP().ToString();
    }

    private void LevelUp()
    {
        level++;
        playerPrevXP = GetXP();
        xpToNextLevel += 10;

        playerMaxHP += 10;
        UpdateHPText();
        healthManager.UpdateHPBar();
        if (levelUpSound)
        {
            audioSource.PlayOneShot(levelUpSound);
        }

        if (playerLevelText != null)
        {
            playerLevelText.text = "Level: " + level;
        }

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
            
            playerCurrentHP += healingAmount;
            if (healSound)
            {
                audioSource.PlayOneShot(healSound);
            }
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

    public void SetPlayerCurrentHP(float newHP)
    {
        playerCurrentHP = newHP;
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
        Debug.Log("Days survived: " + daysSurvived);
    }

    public void UpdateEnemiesKilled()
    {
        enemiesKilled = enemyCounter.GetEnemiesKilled();
        Debug.Log("Enemies killed: " + enemiesKilled);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void LoadPlayerStats()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string jsonData = PlayerPrefs.GetString(PlayerPrefsKey);
            PlayerStatsData statsData = JsonUtility.FromJson<PlayerStatsData>(jsonData);

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

        string jsonData = JsonUtility.ToJson(statsData);
        PlayerPrefs.SetString(PlayerPrefsKey, jsonData);

        Debug.Log("Player stats saved.");
    }

    public static PlayerStats GetInstance()
    {
        return instance;
    }

    public void ResetPlayerStats()
    {
        xp = 0;
        level = 1;
        xpToNextLevel = 10;
        playerCurrentHP = 10f;
        playerMaxHP = 10f;
        playerCurrentXP = 0;
        playerPrevXP = 0;
        daysSurvived = 0;
        enemiesKilled = 0;

        SavePlayerStats();

        Debug.Log("Player stats reset to initial values.");
    }
}
