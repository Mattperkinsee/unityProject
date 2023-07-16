using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign this in the Inspector
    public float respawnTime = 1f; // Time in seconds between respawns
    private Vector3 initialPosition; // Store the initial position of the spawner
    private Transform gameCanvas; // Reference to the GameCanvas transform

    void Start()
    {
        gameCanvas = GameObject.Find("GameCanvas").transform; // Assign the GameCanvas reference
        Invoke("SpawnEnemy", 0f); // Delay the first spawn by 3 seconds
        Debug.Log("Spawn enemy!");
    }


   public void SpawnEnemy()
{
    // Calculate the random position within the canvas bounds
    float canvasWidth = gameCanvas.GetComponent<RectTransform>().rect.width;
    float canvasHeight = gameCanvas.GetComponent<RectTransform>().rect.height;
    float minX = -canvasWidth / 3.5f;
    float maxX = canvasWidth / 3.5f;
    float minY = -canvasHeight / 3.5f;
    float maxY = canvasHeight / 3.5f;

    // Calculate the random position within the specified ranges
    Vector3 randomPosition = new Vector3(
        Random.Range(minX, maxX),
        Random.Range(minY, maxY),
        0f);

    // Convert the local position to a world position
    Vector3 worldPosition = gameCanvas.TransformPoint(randomPosition);

    // Instantiate the enemy prefab as a child of the GameCanvas
    GameObject enemyInstance = Instantiate(enemyPrefab, gameCanvas);

    // Set the position to the world position
    enemyInstance.transform.position = worldPosition;

    // Set the local rotation to 180 degrees around the Y-axis
    enemyInstance.transform.localRotation = Quaternion.Euler(0, 180, 0);

    // Give the new enemy a reference to the spawner so it can trigger a respawn
    enemyInstance.GetComponent<Enemy>().spawner = this;
}





    // This method will be called by the Enemy script when the enemy dies
    public void RespawnEnemy()
    {
        Debug.Log("Respawning enemy!"); // Add this debug log

        // Invoke the SpawnEnemy method after a delay
        Invoke("SpawnEnemy", respawnTime);
    }
}
