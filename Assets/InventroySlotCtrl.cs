using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventroySlotCtrl : MonoBehaviour
{
    [SerializeField] private GameObject LockedSlot;
    [SerializeField] private GameObject UnlockedSlot;
    [SerializeField] private GameObject EmptySlot;
    [SerializeField] private GameObject UsedSlot;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemCountText;
    [SerializeField] private Image itemsImage;

    private InventorySlot slotData;
    private int baseUnlockedSlotCount = 15;
    private User user;

    private void Start()
    {
        if (!user)
        {
            user = User.Instance;
            if (!user)
            {
                throw new Exception("User is missing");
            }
        }
        if (ConfigManager.Instance)
        {
            baseUnlockedSlotCount = ConfigManager.Instance.BaseInventorySlotCount;
        }
    }

    private void SetInventorySlot(InventorySlot slotData)
    {
        this.slotData = slotData;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int siblingIndex = gameObject.transform.GetSiblingIndex();
        int currentUnlockedSlotCount = baseUnlockedSlotCount + user.PurchasedInventroySlotCount;
        bool isUnlocked = siblingIndex < currentUnlockedSlotCount;
        if (LockedSlot)
        {
            LockedSlot.SetActive(!isUnlocked);
        }
        if (UnlockedSlot)
        {
            UnlockedSlot.SetActive(isUnlocked);
        }
        if (EmptySlot)
        {
            EmptySlot.SetActive(slotData.Count == 0);
        }
        if (UsedSlot)
        {
            UsedSlot.SetActive(slotData.Count > 0);
            if (itemNameText)
            {

            }
            if (itemCountText)
            {

            }
            if (itemsImage)
            {

            }
        }
    }
}
