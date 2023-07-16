using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject damageTextPrefab;  // Prefab for displaying damage text
    public float damageAmount = 10f;  // The amount of damage to display
    public RectTransform canvasRectTransform;  // RectTransform of your canvas
    public AudioSource audioSource;  // The audio source component that will play the sound
    public AudioClip collisionSound;  // The audio clip that contains the sound
    public int maxLinePoints = 10;
    private List<Enemy> collidedEnemies = new List<Enemy>();
    private bool isDrawing;

    public string sortingLayerName = "Line";  // Sorting layer name for the line
    public int sortingOrder = 1;  // Sorting order for the line

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

        // Rest of your Start method...
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
            RaycastHit2D hit = Physics2D.Linecast(startPos, mousePos);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    // Collided with enemy!
                    // Debug.Log("Collision detected with: " + hit.collider.gameObject.name);
                    // Cause damage to the enemy
                    Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();

                    if (enemy != null && !collidedEnemies.Contains(enemy))
                    {
                        // Start the coroutine to clear the collided enemies list after 0.5 seconds
                        StartCoroutine(ClearCollidedEnemiesAfterDelay(0.3f));

                        // Play the collision sound
                        if (collisionSound != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(collisionSound);
                        }

                        collidedEnemies.Add(enemy);
                        // Get the collision point from the collision event data
                        Vector3 collisionPoint = hit.transform.position;

                        // Cause damage to the enemy at the collision point
                        enemy.TakeDamage(damageAmount);

                        // Display the damage at the collision point
                        DisplayDamage(collisionPoint);
                    }
                }
            }
        }
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
