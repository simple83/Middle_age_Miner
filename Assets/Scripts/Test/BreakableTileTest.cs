using UnityEngine;

public class BreakableTileTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0; // 2D 월드에서 Z는 0으로 고정

            if (BreakableTileManager.Instance != null)
            {
                BreakableTileManager.Instance.DamageTileAt(worldPos);
            }
        }
    }
}
