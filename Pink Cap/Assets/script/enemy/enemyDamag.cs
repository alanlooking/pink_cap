using UnityEngine;

public class enemydamag : MonoBehaviour
{
    public float bounceForce = 7f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

      
        if (collision.transform.position.y > transform.position.y + 0.3f)
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.linearVelocity =
                    new Vector2(playerRb.linearVelocity.x, bounceForce);
            }

            animator.SetBool("isDead", true);

            GetComponent<Collider2D>().enabled = false;
            GetComponent<EnemyPatrol>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;

            Destroy(gameObject, 0.65f);
        }
        else
        {
  
            PlayerDeath playerDeath = collision.gameObject.GetComponent<PlayerDeath>();

            if (playerDeath != null)
            {
                playerDeath.Die();
            }
        }
    }
}