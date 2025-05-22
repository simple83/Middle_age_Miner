using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class BreakableTileRegistrar : MonoBehaviour
{
    void Awake()
    {
        var tilemap = GetComponent<Tilemap>();
        BreakableTileManager.Instance?.RegisterTilemap(tilemap);
    }
}
