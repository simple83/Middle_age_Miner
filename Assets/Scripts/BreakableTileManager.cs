using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableTileManager : MonoBehaviour
{
    public static BreakableTileManager Instance { get; private set; }
    private Tilemap tilemap;
    private Dictionary<Tilemap, Dictionary<Vector3Int, BreakableTileData>> tilemapData
        = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void RegisterTilemap(Tilemap tilemap)
    {
        this.tilemap = tilemap;
        var data = new Dictionary<Vector3Int, BreakableTileData>();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) is BreakableTile tile)
            {
                tilemap.SetTileFlags(pos, TileFlags.None);
                data[pos] = new BreakableTileData(pos, tile);
            }
        }

        tilemapData[tilemap] = data;
    }

    public void DamageTileAt(Vector3 worldPos)
    {
        if (tilemap == null) return;
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        if (!tilemapData.ContainsKey(tilemap)) return;
        if (!tilemapData[tilemap].ContainsKey(cellPos)) return;

        var tileData = tilemapData[tilemap][cellPos];
        tileData.durability--;

        UpdateTileSprite(tilemap, tileData);

        if (tileData.durability <= 0)
        {
            tilemap.SetTile(cellPos, null);
            tilemapData[tilemap].Remove(cellPos);
        }
    }

    void UpdateTileSprite(Tilemap tilemap, BreakableTileData tileData)
    {
        var tile = tileData.tileAsset;
        float p = tileData.Progress;
        Sprite newSprite = tile.intactSprite;

        if (p > 0.66f) newSprite = tile.brokenSprite;
        else if (p > 0.33f) newSprite = tile.crackedSprite;

        var newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = newSprite;
        newTile.colliderType = Tile.ColliderType.Grid;
        tilemap.SetTile(tileData.position, newTile);
    }
}

public class BreakableTileData
{
    public Vector3Int position;
    public int durability;
    public BreakableTile tileAsset;

    public BreakableTileData(Vector3Int pos, BreakableTile tile)
    {
        position = pos;
        tileAsset = tile;
        durability = tile.maxDurability;
    }

    public float Progress => 1f - (float)durability / tileAsset.maxDurability;
}