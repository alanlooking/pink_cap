using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 7f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            move = 1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            move = -1f;
        }

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            isGrounded = false;
        }

        bool isMoving = move != 0f;
        bool isJumping = !isGrounded;

        animator.enabled = !isMoving && !isJumping;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}