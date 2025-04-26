using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotCtrl : MonoBehaviour
{
    [SerializeField] private GameObject LockedSlot;
    [SerializeField] private GameObject UnlockedSlot;
    [SerializeField] private GameObject EmptySlot;
    [SerializeField] private GameObject UsedSlot;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemCountText;
    [SerializeField] private Image itemsImage;

    public void SetUnlocked(bool isUnlocked)
    {
        if (LockedSlot)
        {
            LockedSlot.SetActive(!isUnlocked);
        }
        if (UnlockedSlot)
        {
            UnlockedSlot.SetActive(isUnlocked);
        }
    }

    public void UpdateInventorySlot(InventorySlot slotData)
    {
        if (EmptySlot)
        {
            EmptySlot.SetActive(slotData.Count <= 0);
        }
        if (UsedSlot)
        {
            UsedSlot.SetActive(slotData.Count > 0);
            if (itemNameText)
            {
                itemNameText.text = slotData.ItemConfig.name;
            }
            if (itemCountText)
            {
                itemCountText.text = slotData.Count.ToString();
            }
            if (itemsImage)
            {
                itemsImage.sprite = SpriteManager.Instance.GetItemThumbnail(slotData.ItemConfig.thumbnailIndex);
            }
        }
    }

    public void OnClickSlot()
    {
        Debug.Log("slot clicked");
    }
}
