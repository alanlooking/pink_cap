using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 7f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        if (move > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (move < 0)
        {
            spriteRenderer.flipX = true;
        }

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        animator.SetBool("isRunning", move != 0f);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            isGrounded = false;
        }
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