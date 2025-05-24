using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class BreakableTileRegistrar : MonoBehaviour
{
    void Start()
    {
        var tilemap = GetComponent<Tilemap>();
        var manager = BreakableTileManager.Instance;
        if (manager != null)
        {
            manager.RegisterTilemap(tilemap);
        }
    }
}
