using UnityEngine;
using UnityEngine.Tilemaps;

public class enemydamag : MonoBehaviour
{
    [Header("Настройки плиток-ловушек")]
    [SerializeField] private Tilemap tilemap;
    [Tooltip("Сюда перетащи только плитки СТАДИИ 3 (которые горят постоянно)")]
    [SerializeField] private TileBase[] lethalTiles;

    [Header("Проверка пола")]
    [SerializeField] private float checkDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (tilemap == null)
        {
            tilemap = FindFirstObjectByType<Tilemap>();
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckForLethalTile();
        }
    }

    private void CheckForLethalTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);

        if (hit.collider != null)
        {
            Vector3 hitPoint = hit.point + (Vector2.down * 0.02f);
            Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);
            TileBase currentTile = tilemap.GetTile(cellPosition);

            if (currentTile == null) return;

            for (int i = 0; i < lethalTiles.Length; i++)
            {
                if (currentTile == lethalTiles[i])
                {
                    Die();
                    break;
                }
            }
        }
    }

    private void Die()
    {
        isDead = true;

        if (UiAudioManager.Instance != null)
        {
            UiAudioManager.Instance.PlayRobotDeath();
        }

        animator.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false;

        if (GetComponent<EnemyPatrol>() != null)
            GetComponent<EnemyPatrol>().enabled = false;

        GetComponent<Rigidbody2D>().simulated = false;

        Hatch hatch = FindFirstObjectByType<Hatch>();
        if (hatch != null)
        {
            hatch.OnEnemyDestroyed();
        }

        Destroy(gameObject, 0.65f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDeath playerDeath = collision.gameObject.GetComponent<PlayerDeath>();

            if (playerDeath != null)
            {
                playerDeath.Die();
            }
        }
    }
}