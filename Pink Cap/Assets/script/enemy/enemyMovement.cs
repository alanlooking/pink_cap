using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;

    public Transform leftPoint;
    public Transform rightPoint;
    private SpriteRenderer spriteRenderer;

    private bool movingRight = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (movingRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, rightPoint.position) < 0.05f)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, leftPoint.position) < 0.05f)
            {
                movingRight = true;
            }
        }

        if (movingRight)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

    }
}