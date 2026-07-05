using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class Hatch : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private Animator animator;

    [Header("Настройки открытия")]
    [SerializeField] private float openHeight = 2f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float animationDuration = 1f;

    private int totalTilesToActivate = 0;
    private int activeEnemiesCount = 0;
    private bool shouldOpen = false;
    private Vector3 targetPosition;

    private HashSet<Vector3Int> activatedPositions = new HashSet<Vector3Int>();

    void Start()
    {
        targetPosition = transform.position + Vector3.up * openHeight;
        activeEnemiesCount = FindObjectsByType<enemydamag>(FindObjectsSortMode.None).Length;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (targetTilemap == null)
        {
            targetTilemap = FindFirstObjectByType<Tilemap>();
        }

        PlayerTileAnimator playerAnimator = FindFirstObjectByType<PlayerTileAnimator>();

        if (targetTilemap != null && playerAnimator != null)
        {
            targetTilemap.CompressBounds();
            BoundsInt bounds = targetTilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (targetTilemap.HasTile(pos))
                {
                    TileBase tile = targetTilemap.GetTile(pos);
                    if (playerAnimator.IsInitialTile(tile))
                    {
                        totalTilesToActivate++;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (shouldOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                enabled = false;
            }
        }
    }

    public void OnTileActivated(Vector3Int position)
    {
        if (activatedPositions.Add(position))
        {
            CheckConditions();
        }
    }

    public void OnEnemyDestroyed()
    {
        activeEnemiesCount--;
        CheckConditions();
    }

    private void CheckConditions()
    {
        if (activatedPositions.Count >= totalTilesToActivate && activeEnemiesCount <= 0 && !shouldOpen)
        {
            StartCoroutine(OpenProcess());
        }
    }

    private IEnumerator OpenProcess()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        yield return new WaitForSeconds(animationDuration);

        shouldOpen = true;
    }
}