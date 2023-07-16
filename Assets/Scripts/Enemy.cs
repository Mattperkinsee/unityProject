using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    Coroutine attackLoop;
    public float attackInterval = 2f; // Time in seconds between each attack
    public float attackDamage = 2f; // Amount of health to subtract on each attack

    public float enemyHealth = 100f;
    public Image enemyHealthBar; // Reference to the enemy health bar UI element
    public GameObject enemyHealthBarObject;
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public GameObject enemySprite; // Child object of EnemyObject that contains the visual representation of the enemy

    public HealthManager healthManager; // Assign the HealthManager in the inspector
    public EnemySpawner spawner;
    public EnemyCounter enemyCounter;

    private bool hasSpawned = false; // Flag to track if enemy has finished spawning

    public float minInitialVelocity = -25f;  // The minimum initial x velocity
    public float maxInitialVelocity = -5f;   // The maximum initial x velocity
    private float initialVelocity;

    private Rigidbody2D rb;

    public PlayerStats playerStats;

    private void Awake()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerStats = PlayerStats.GetInstance();
        enemyCounter = FindObjectOfType<EnemyCounter>();
        spawner = FindObjectOfType<EnemySpawner>();
        healthManager = FindObjectOfType<HealthManager>();
        // Start the attack loop
        attackLoop = StartCoroutine(AttackLoop());

        // Disable collision detection initially
        SetCollisionDetection(true);
        // Randomize the initial velocity within the specified range
        initialVelocity = Random.Range(minInitialVelocity, maxInitialVelocity);
        // Start the coroutine to give the enemy an initial velocity
        StartCoroutine(GiveInitialVelocity(initialVelocity));

        Debug.Log("Enemy Start called."); // Debug log to check if Start method is executed
    }

    private IEnumerator GiveInitialVelocity(float initialVelocity)
    {
        // Give the enemy an initial x velocity
        rb.velocity = new Vector2(initialVelocity, rb.velocity.y);

        // Wait for 2 seconds
        yield return new WaitForSeconds(1f);

        // After 2 seconds, set the x velocity back to 0
        rb.velocity = new Vector2(0, rb.velocity.y);
    }


    public void TakeDamage(float damage)
    {
        if (hasSpawned) // Only take damage after enemy has finished spawning
        {
            enemyHealth -= damage;

            // Update the enemy's health bar
            if (enemyHealthBar != null)
            {
                enemyHealthBar.fillAmount = enemyHealth / 100f;
            }

            if (enemyHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        // Call the IncrementEnemyCount() method on the enemyCounter script
        enemyCounter.IncrementEnemyCount();
        // Stop the attack coroutine
        StopCoroutine(attackLoop);

        //Increment Player XP
        playerStats.IncrementXP(5);

        // Destroy the enemy object
        Destroy(gameObject);

        // If the spawner is not null, call RespawnEnemy
        if (spawner != null)
        {
            spawner.RespawnEnemy();
            spawner.RespawnEnemy(); //exponential enemies!
        }
        else
        {
            Debug.Log("Spawner is null!");
        }
    }



    IEnumerator AttackLoop()
    {
        while (true)
        {
            // Generate a random attack interval between 2 and 5 seconds
            float randomInterval = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomInterval);

            // Call the TakeDamage method in the HealthManager
            healthManager.TakeDamage(attackDamage);

            Debug.Log("Enemy attacked player!");
        }
    }

    void OnEnable()
    {
        // Enable collision detection when the enemy becomes active
        SetCollisionDetection(true);
        hasSpawned = true;
    }

    void OnDisable()
    {
        // Disable collision detection when the enemy becomes inactive
        SetCollisionDetection(false);
        hasSpawned = false;
    }

    void SetCollisionDetection(bool enabled)
    {
        // Enable/disable the line collider based on the flag
        Collider2D lineCollider = GetComponent<Collider2D>();
        if (lineCollider != null)
        {
            lineCollider.enabled = enabled;
        }
    }
}
