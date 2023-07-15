using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    // Start is called before the first frame update

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.Play("Idle");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            animator.Play("Walk");
        }
    }
}
