using UnityEngine;

public class movement : MonoBehaviour
{
    [Header("Настройки скорости")]
    public float speed = 5f;
    public float jump = 7f;

    [Header("Инерция")]
    public float acceleration = 20f;
    public float deceleration = 15f;

    [Header("Проверка стен")]
    [SerializeField] private float wallCheckDistance = 0.35f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private bool isGrounded = true;
    private float currentHorizontalSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        if (wallCheckDistance == 0.35f && playerCollider != null)
        {
            wallCheckDistance = (playerCollider.bounds.extents.x) + 0.05f;
        }
    }

    void Update()
    {
        float targetMove = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
            targetMove = 1f;
        if (Input.GetKey(KeyCode.LeftArrow))
            targetMove = -1f;

        if (targetMove > 0)
            spriteRenderer.flipX = false;
        if (targetMove < 0)
            spriteRenderer.flipX = true;

        bool hitsWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, groundLayer);
        bool hitsWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, groundLayer);

        if (hitsWallRight && currentHorizontalSpeed > 0)
        {
            targetMove = 0;
            currentHorizontalSpeed = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else if (hitsWallLeft && currentHorizontalSpeed < 0)
        {
            targetMove = 0;
            currentHorizontalSpeed = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        float targetSpeed = targetMove * speed;

        float rate = (targetMove != 0 && Mathf.Sign(targetMove) == Mathf.Sign(currentHorizontalSpeed))
            ? acceleration
            : deceleration;

        currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, targetSpeed, rate * Time.deltaTime);

        rb.linearVelocity = new Vector2(currentHorizontalSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
            isGrounded = false;
        }

        animator.SetBool("isRunning", Mathf.Abs(currentHorizontalSpeed) > 0.1f && isGrounded);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallCheckDistance);
    }
}