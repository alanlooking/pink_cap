using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerTileAnimator : MonoBehaviour
{
    [System.Serializable]
    public struct BlockAnimationData
    {
        public string blockName; // Для удобства в инспекторе (например, "Земля", "Металл")
        public TileBase state1;  // Исходный блок (выключен)
        public TileBase state2;  // Кадр анимации (загорается)
        public TileBase state3;  // Финальный блок (горит постоянно)
    }

    [Header("Компоненты")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private BlockAnimationData[] blocks; // Твои 3 вида блоков

    [Header("Настройки анимации")]
    [Tooltip("Скорость перехода со 2 на 3 стадию (в секундах)")]
    [SerializeField] private float animationDelay = 0.1f;

    [Header("Проверка пола (Raycast)")]
    [SerializeField] private float checkDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        CheckFloorTile();
    }

    private void CheckFloorTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);

        if (hit.collider != null)
        {
            // Смещение внутрь тайла для точности
            Vector3 hitPoint = hit.point + (Vector2.down * 0.05f);
            Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);
            TileBase currentTile = tilemap.GetTile(cellPosition);

            if (currentTile == null) return;

            // Ищем, относится ли текущий тайл к начальной стадии какого-то из 3-х видов блоков
            for (int i = 0; i < blocks.Length; i++)
            {
                if (currentTile == blocks[i].state1)
                {
                    // Шаг 1: Мгновенно включаем вторую стадию
                    tilemap.SetTile(cellPosition, blocks[i].state2);

                    // Шаг 2: Запускаем таймер, который сам додует анимацию до 3 стадии
                    StartCoroutine(PlayFinalFrame(cellPosition, blocks[i].state3));

                    break; // Выходим из цикла, так как блок уже найден и изменен
                }
            }
        }
    }

    // Этот мини-таймер срабатывает один раз и гарантирует, что блок станет третьим
    private IEnumerator PlayFinalFrame(Vector3Int position, TileBase finalTile)
    {
        yield return new WaitForSeconds(animationDelay);

        // На всякий случай проверяем, существует ли еще тайл (вдруг игрок его сломал механикой игры)
        if (tilemap.GetTile(position) != null)
        {
            tilemap.SetTile(position, finalTile);
        }
    }
}