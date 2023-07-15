using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject damageTextPrefab;  // Prefab for displaying damage text
    public float damageAmount = 10f;  // The amount of damage to display
    public RectTransform canvasRectTransform;  // RectTransform of your canvas
    public AudioSource audioSource;  // The audio source component that will play the sound
    public AudioClip collisionSound;  // The audio clip that contains the sound

    private bool isDrawing;

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

    private void StartDrawing()
    {
        isDrawing = true;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, GetMouseWorldPosition());
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
    }

    private IEnumerator HideLineAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;
    }

    private void UpdateLinePositions()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        lineRenderer.SetPosition(1, mousePos);

        // Update collider position to match line end point
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider)
        {
            boxCollider.transform.position = mousePos;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    // Collision detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collision detected with: " + other.gameObject.name);

            // Play the collision sound
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }

            // Cause damage to the enemy
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                // Get the collision point from the collision event data
                Vector3 collisionPoint = other.transform.position;

                // Cause damage to the enemy at the collision point
                enemy.TakeDamage(damageAmount);

                // Display the damage at the collision point
                DisplayDamage(collisionPoint);
            }
        }
    }



    // Displays damage at the specified position
    private void DisplayDamage(Vector3 position)
    {
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
