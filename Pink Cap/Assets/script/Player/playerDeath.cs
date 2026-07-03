using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public float restartDelay = 1f;

    private bool isDead = false;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private movement movementScript;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        movementScript = GetComponent<movement>();
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        animator.SetBool("isDeath", true);

        movementScript.enabled = false;
        playerCollider.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        Destroy(gameObject, 0.65f);

        Invoke(nameof(RestartLevel), restartDelay);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}