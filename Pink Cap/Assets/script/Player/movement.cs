using UnityEngine;

public class movement : MonoBehaviour
{
    [Header("Настройки скорости")]
    public float speed = 14f;
    public float jump = 13f;

    [Header("Инерция")]
    [Tooltip("Как быстро разгоняется (чем выше, тем быстрее наберет макс. скорость)")]
    public float acceleration = 20f;
    [Tooltip("Как быстро тормозит / скользит (чем ниже, тем длиннее тормозной путь)")]
    public float deceleration = 15f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;
    private float currentHorizontalSpeed = 0f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
}