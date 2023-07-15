using System.Collections;
using UnityEngine;
using TMPro;

public class DamageTextController : MonoBehaviour
{
    public float speed = 1.0f;  // The speed at which the text should move upwards
    public float lifetime = 1.0f;  // How long the text should exist before disappearing

    void Start()
    {
        StartCoroutine(DestroyAfterTime(lifetime));
    }

    void Update()
    {
        // Move upwards
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
