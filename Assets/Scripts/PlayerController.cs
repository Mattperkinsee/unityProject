using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 400f;
    private bool isJumping = false;
    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move left and right
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        // Flip the sprite to the correct direction
        if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveHorizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Jump when Space is pressed
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player lands on the ground, they can jump again
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }
}
