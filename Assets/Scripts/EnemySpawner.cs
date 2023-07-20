using UnityEngine;
using UnityEngine.Audio;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Assign the enemy prefabs in the Inspector
    public float respawnTime = 1f; // Time in seconds between respawns
    private Vector3 initialPosition; // Store the initial position of the spawner
    private Transform gameCanvas; // Reference to the GameCanvas transform

    public int maxEnemies = 10; // This can be any number you want.
    private int currentEnemyCount = 0;

    void Start()
    {
        gameCanvas = GameObject.Find("GameCanvas").transform; // Assign the GameCanvas reference
        Invoke("SpawnEnemy", 0f); // Delay the first spawn 
        Debug.Log("Spawn enemy!");
    }

    public AudioSource audioSource;  // The audio source component that will play the sound
    public AudioClip deathSound;  // The audio clip that contains the sound

    public void SpawnEnemy()
    {
        // audioSource.PlayOneShot(deathSound);
        if (currentEnemyCount >= maxEnemies) return; // Don't spawn if there are already maxEnemies in the scene

        // Calculate the random position within the canvas bounds
        float canvasWidth = gameCanvas.GetComponent<RectTransform>().rect.width;
        float canvasHeight = gameCanvas.GetComponent<RectTransform>().rect.height;

        float offsetX = 100f;  // Offset to spawn the enemy off-screen. Adjust this value as needed.

        float maxX = canvasWidth / 2f + offsetX;  // Spawn the enemy off-screen to the right
        // For x, we now just use maxX, so that the enemy always spawns off-screen to the right
        float x = maxX;

        float minY = -canvasHeight * (385f / 810f);
        float maxY = -canvasHeight * (385f / 810f);
        float y = Random.Range(minY, maxY);

        Debug.Log("Canvas Height" + canvasHeight);
        Vector3 spawnPosition = new Vector3(x, y, 0);

        // Convert the local position to a world position
        Vector3 worldPosition = gameCanvas.TransformPoint(spawnPosition);

        // Choose a random enemy prefab from the list
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Instantiate the enemy prefab as a child of the GameCanvas
        GameObject enemyInstance = Instantiate(enemyPrefab, gameCanvas);
        currentEnemyCount++; // Increment the enemy count
        Debug.Log("Enemy name:" + enemyPrefab.name);
        // Check if the selected prefab is "Mob 2-1"
        if (enemyPrefab.name == "RedMushroomMob")
        {
            // Scale the enemy randomly by +-10 on the x, y, and z axes
            Vector3 randomScale = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );
            enemyInstance.transform.localScale += randomScale;
        }

        // Set the position to the world position
        enemyInstance.transform.position = worldPosition;

        // Set the local rotation to 180 degrees around the Y-axis
        // enemyInstance.transform.localRotation = Quaternion.Euler(0, 180, 0);

        // Give the new enemy a reference to the spawner so it can trigger a respawn
        enemyInstance.GetComponent<Enemy>().spawner = this;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;  // Decrement the enemy count when an enemy is destroyed.
        audioSource.PlayOneShot(deathSound);
    }

    // This method will be called by the Enemy script when the enemy dies
    public void RespawnEnemy()
    {
        Debug.Log("Respawning enemy!"); // Add this debug log

        // Invoke the SpawnEnemy method after a delay
        Invoke("SpawnEnemy", respawnTime);
    }

   
}
