using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign this in the Inspector
    public float respawnTime = 1f; // Time in seconds between respawns
    private Vector3 initialPosition; // Store the initial position of the spawner
    private Transform gameCanvas; // Reference to the GameCanvas transform

    public int maxEnemies = 10; // This can be any number you want.
    private int currentEnemyCount = 0;
    void Start()
    {
        gameCanvas = GameObject.Find("GameCanvas").transform; // Assign the GameCanvas reference
        Invoke("SpawnEnemy", 0f); // Delay the first spawn by 3 seconds
        Debug.Log("Spawn enemy!");
    }


    public void SpawnEnemy()
    {

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

        // Instantiate the enemy prefab as a child of the GameCanvas
        GameObject enemyInstance = Instantiate(enemyPrefab, gameCanvas);
        currentEnemyCount++; // Increment the enemy count
        // Set the position to the world position
        enemyInstance.transform.position = worldPosition;

        // Set the local rotation to 180 degrees around the Y-axis
        enemyInstance.transform.localRotation = Quaternion.Euler(0, 180, 0);

        // Give the new enemy a reference to the spawner so it can trigger a respawn
        enemyInstance.GetComponent<Enemy>().spawner = this;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;  // Decrement the enemy count when an enemy is destroyed.
    }






    // This method will be called by the Enemy script when the enemy dies
    public void RespawnEnemy()
    {
        Debug.Log("Respawning enemy!"); // Add this debug log

        // Invoke the SpawnEnemy method after a delay
        Invoke("SpawnEnemy", respawnTime);
    }
}
