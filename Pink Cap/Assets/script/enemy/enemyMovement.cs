using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform leftPoint;
    public Transform rightPoint;

    private SpriteRenderer spriteRenderer;
    private bool movingRight = true;

    private Vector2 leftTargetPosition;
    private Vector2 rightTargetPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (leftPoint != null) leftTargetPosition = leftPoint.position;
        if (rightPoint != null) rightTargetPosition = rightPoint.position;
    }

    void Update()
    {
        if (movingRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, rightTargetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, rightTargetPosition) < 0.1f)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, leftTargetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, leftTargetPosition) < 0.1f)
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