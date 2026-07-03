using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 7f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
            move = 1f;

        if (Input.GetKey(KeyCode.LeftArrow))
            move = -1f;

        if (move > 0)
            spriteRenderer.flipX = false;

        if (move < 0)
            spriteRenderer.flipX = true;

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            isGrounded = false;
        }

        animator.SetBool("isRunning", move != 0 && isGrounded);
        animator.SetBool("isJumping", !isGrounded);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}