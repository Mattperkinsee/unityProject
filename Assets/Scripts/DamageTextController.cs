using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    public float speed = 5f;  // The speed at which the text should move upwards
    public float distanceThreshold = 2f;  // The distance the text needs to travel before being destroyed
    public Vector3 initialOffset = new Vector3(1f, 3f, 0f);  // The initial offset to apply to the text

    private Vector3 initialPosition;  // The initial position of the text

    private void Start()
    {
        initialPosition = transform.position + initialOffset;
        transform.position = initialPosition;
    }

    private void Update()
    {
        // Move upwards
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Calculate the distance traveled from the initial position
        float distance = Vector3.Distance(transform.position, initialPosition);

        // Check if the distance threshold has been reached
        if (distance >= distanceThreshold)
        {
            // Debug.Log("DESTROY TEXT!");
            Destroy(gameObject);
        }
    }
}
