using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Hatch : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private Tilemap targetTilemap;

    [Header("Настройки открытия")]
    [SerializeField] private float openHeight = 2f;
    [SerializeField] private float openSpeed = 2f;

    private int totalTilesToActivate = 0;
    private int activeEnemiesCount = 0;
    private bool shouldOpen = false;
    private Vector3 targetPosition;

    private HashSet<Vector3Int> activatedPositions = new HashSet<Vector3Int>();

    void Start()
    {
        targetPosition = transform.position + Vector3.up * openHeight;
        activeEnemiesCount = FindObjectsByType<enemydamag>(FindObjectsSortMode.None).Length;

        if (targetTilemap == null)
        {
            targetTilemap = FindFirstObjectByType<Tilemap>();
        }

        PlayerTileAnimator animator = FindFirstObjectByType<PlayerTileAnimator>();

        if (targetTilemap != null && animator != null)
        {
            targetTilemap.CompressBounds();
            BoundsInt bounds = targetTilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (targetTilemap.HasTile(pos))
                {
                    TileBase tile = targetTilemap.GetTile(pos);
                    if (animator.IsInitialTile(tile))
                    {
                        totalTilesToActivate++;
                    }
                }
            }
        }

        Debug.Log($"[Люк] Старт уровня! Найдено ВСЕГО блоков на карте: {totalTilesToActivate}. Врагов: {activeEnemiesCount}");
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
            Debug.Log($"[Люк] Блок активирован! Осталось зажечь: {totalTilesToActivate - activatedPositions.Count}");
            CheckConditions();
        }
    }

    public void OnEnemyDestroyed()
    {
        activeEnemiesCount--;
        Debug.Log($"[Люк] Враг повержен! Осталось врагов: {activeEnemiesCount}");
        CheckConditions();
    }

    private void CheckConditions()
    {
        if (activatedPositions.Count >= totalTilesToActivate && activeEnemiesCount <= 0)
        {
            shouldOpen = true;
            Debug.Log("[Люк] ВСЕ УСЛОВИЯ ВЫПОЛНЕНЫ! ОТКРЫВАЮСЬ!");
        }
    }
}