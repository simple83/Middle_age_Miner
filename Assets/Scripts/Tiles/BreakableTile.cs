using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/BreakableTile")]
public class BreakableTile : Tile
{
    public int maxDurability = 3;

    public Sprite intactSprite;   // 초기 상태
    public Sprite crackedSprite;  // 중파 상태
    public Sprite brokenSprite;   // 거의 파괴된 상태
}
