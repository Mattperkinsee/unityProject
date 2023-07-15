using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    public float speed = 1.0f;  // The speed at which the text should move upwards
    public float distanceThreshold = 10.0f;  // The distance the text needs to travel before being destroyed

    private Vector3 initialPosition;  // The initial position of the text

    private void Start()
    {
        initialPosition = transform.position;
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
            Destroy(gameObject);
        }
    }
}
