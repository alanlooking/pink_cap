using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class PlayerTileAnimator : MonoBehaviour
{
    [System.Serializable]
    public struct BlockAnimationData
    {
        public string blockName;
        public TileBase state1;
        public TileBase state2;
        public TileBase state3;
    }

    [Header("Компоненты")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private BlockAnimationData[] blocks;

    [Header("Настройки анимации")]
    [SerializeField] private float animationDelay = 0.1f;

    [Header("Проверка пола (Raycast)")]
    [SerializeField] private float checkDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private HashSet<Vector3Int> animatedTiles = new HashSet<Vector3Int>();
    private Hatch hatch;

    void Start()
    {
        hatch = FindFirstObjectByType<Hatch>();

        if (tilemap == null)
        {
            tilemap = FindFirstObjectByType<Tilemap>();
        }
    }

    void Update()
    {
        CheckFloorTile();
    }

    public bool IsInitialTile(TileBase tile)
    {
        if (tile == null) return false;
        for (int i = 0; i < blocks.Length; i++)
        {
            if (tile == blocks[i].state1) return true;
        }
        return false;
    }

    private void CheckFloorTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);

        if (hit.collider != null)
        {
            Vector3 hitPoint = hit.point + (Vector2.down * 0.02f);
            Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);
            TileBase currentTile = tilemap.GetTile(cellPosition);

            if (currentTile == null) return;
            if (animatedTiles.Contains(cellPosition)) return;

            for (int i = 0; i < blocks.Length; i++)
            {
                if (currentTile == blocks[i].state1)
                {
                    animatedTiles.Add(cellPosition);
                    tilemap.SetTile(cellPosition, blocks[i].state2);
                    StartCoroutine(PlayFinalFrame(cellPosition, blocks[i].state3));
                    break;
                }
            }
        }
    }

    private IEnumerator PlayFinalFrame(Vector3Int position, TileBase finalTile)
    {
        yield return new WaitForSeconds(animationDelay);

        if (tilemap.GetTile(position) != null)
        {
            tilemap.SetTile(position, finalTile);

            if (hatch != null)
            {
                hatch.OnTileActivated(position);
            }
        }

        animatedTiles.Remove(position);
    }
}