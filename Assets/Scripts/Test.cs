using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
  
    public CameraShake cameraShake;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("SHAKE");
            StartCoroutine(cameraShake.Shake(0.15f, 0.4f));
        }
    }
} 