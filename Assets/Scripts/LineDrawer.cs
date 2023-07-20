using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject damageTextPrefab;  // Prefab for displaying damage text
    public float damageAmount = 10f;  // The amount of damage to display
    public RectTransform canvasRectTransform;  // RectTransform of your canvas
    public AudioSource audioSource;  // The audio source component that will play the sound
    public AudioClip collisionSound;  // The audio clip that contains the sound
    public AudioClip bloodSound;  // The audio clip that contains the sound
    public int maxLinePoints = 10;
    private List<Enemy> collidedEnemies = new List<Enemy>();
    private bool isDrawing;

    public string sortingLayerName = "Line";  // Sorting layer name for the line
    public int sortingOrder = 1;  // Sorting order for the line

    public int maxPlayingCollisionSounds = 3; // Maximum number of simultaneous audio clips allowed
    private int currentPlayingCollisionSounds = 0; // Counter variable to track the number of playing sounds
    public GameObject bloodParticlePrefab;

    private Shake shake;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }

        if (isDrawing)
        {
            UpdateLinePositions();
        }

    }

    private void Start()
    {
        // Set the sorting layer and order for the line renderer
        lineRenderer.sortingLayerName = sortingLayerName;
        lineRenderer.sortingOrder = sortingOrder;

        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
    }

    private void StartDrawing()
    {
        isDrawing = true;
        lineRenderer.positionCount = 1;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, GetMouseWorldPosition());

    }
    private IEnumerator ClearCollidedEnemiesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        collidedEnemies.Clear();
    }

    private void StopDrawing()
    {
        isDrawing = false;
        Vector3 endPos = GetMouseWorldPosition();
        lineRenderer.SetPosition(1, endPos);
        StartCoroutine(HideLineAfterSeconds(0));

        // Update edge collider
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        if (edgeCollider)
        {
            Vector2[] points = new Vector2[2];
            points[0] = lineRenderer.GetPosition(0);
            points[1] = endPos;
            edgeCollider.points = points;
        }
        collidedEnemies.Clear(); // Clear the list of collided enemies when you stop drawing
    }

    private IEnumerator HideLineAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;
    }

    private void UpdateLinePositions()
    {
        if (!isDrawing)
        {
            return;
        }

        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 startPos = lineRenderer.GetPosition(0);
        float distance = Vector3.Distance(startPos, mousePos);

        if (distance > .11f) // You can adjust this minimum distance as needed
        {
            if (lineRenderer.positionCount >= maxLinePoints)
            {
                // Shift all points to the left
                for (int i = 0; i < lineRenderer.positionCount - 1; i++)
                {
                    lineRenderer.SetPosition(i, lineRenderer.GetPosition(i + 1));
                }

                // Set the last position to the current mouse position
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
            }
            else
            {
                // Increase the line points and set the last position to the current mouse position
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
            }

            // Add raycasting here
            RaycastHit2D[] hits = Physics2D.LinecastAll(startPos, mousePos);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
                {
                    // Collided with enemy!
                    Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();

                    if (enemy != null && !collidedEnemies.Contains(enemy) && !enemy.IsDamaged())
                    {
                        if (collisionSound != null && audioSource != null && currentPlayingCollisionSounds < maxPlayingCollisionSounds)
                        {
                            // Debug.Log("AudioSource volume: " + audioSource.volume);
                            // audioSource.PlayOneShot(collisionSound, 1f);
                            currentPlayingCollisionSounds++;
                            StartCoroutine(CollisionSoundFinished(collisionSound.length));
                        }
                        audioSource.PlayOneShot(bloodSound, 0.1f);


                        collidedEnemies.Add(enemy);
                        enemy.SetDamaged(true);

                        // Get the collision point from the collision event data
                        Vector3 collisionPoint = hit.point;

                        // Cause damage to the enemy at the collision point after a slight delay
                        StartCoroutine(DamageEnemyWithDelay(enemy, collisionPoint, 0.1f));
                    }
                }
            }
        }
    }

    private IEnumerator CollisionSoundFinished(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Sound has finished playing, do something
        currentPlayingCollisionSounds--;
    }

    private IEnumerator DamageEnemyWithDelay(Enemy enemy, Vector3 collisionPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        // Cause damage to the enemy at the collision point
        shake.CamShake();
        // Instantiate the blood particle system at the collision point and parent it to the enemy
        GameObject bloodPS = Instantiate(bloodParticlePrefab, collisionPoint, Quaternion.identity, enemy.transform);

        // Get the particle system renderer
        ParticleSystemRenderer psr = bloodPS.GetComponent<ParticleSystemRenderer>();

        // Set the sorting layer to be the topmost
        psr.sortingLayerName = "Topmost";

        // Set the order in layer to be higher than the other sprites
        psr.sortingOrder = 10;

        // Set the parent to the enemy transform
        bloodPS.transform.SetParent(enemy.transform);
        // Destroy the blood particle system after 0.3 seconds
        Destroy(bloodPS, .4f);
        enemy.TakeDamage(damageAmount);
        enemy.SetDamaged(false);
        // Display the damage at the collision point
        DisplayDamage(collisionPoint);

        // Clear the collided enemies list after a short delay to prevent immediate collisions
        StartCoroutine(ClearCollidedEnemiesAfterDelay(0.3f));
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private void DisplayDamage(Vector3 position)
    {
        // Debug.Log("Show damage!");
        if (damageTextPrefab == null)
        {
            Debug.LogError("DamageTextPrefab is not assigned. Please assign it in the inspector.");
            return;
        }

        position.y -= 0.2f; // Modify as needed to suit your game's scale
        var damageText = Instantiate(damageTextPrefab, position, Quaternion.identity, canvasRectTransform);

        if (damageText == null)
        {
            Debug.LogError("Failed to instantiate damageText");
            return;
        }

        var textComponent = damageText.GetComponent<TMPro.TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.text = damageAmount.ToString();
        }
        else
        {
            Debug.LogError("Failed to get TextMeshProUGUI component from damageText");
        }
    }
}
