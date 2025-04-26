using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCtrl : MonoBehaviour
{
    private InGameEventManager inGameEventManager;
    private User user;

    private int baseUnlockedSlotCount = 15;
    private int unlockedSlotCount => baseUnlockedSlotCount + user.PurchasedInventroySlotCount;
    private List<InventorySlot> inventorySlots;
    [SerializeField] private InventorySlotCtrl[] slotCtrls;

    private void Awake()
    {
        inGameEventManager = InGameEventManager.Instance;
        if (inGameEventManager)
        {
            inGameEventManager.OpenInventoryEvent.AddListener(() => { gameObject.SetActive(true); });
        }
        else
        {
            throw new Exception("ingamemanager is missing");
        }
        gameObject.SetActive(false);
    }

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
            InitalizeInventory();
            UpdateSlotLockedState();
        }
        else
        {
            throw new Exception("fail to initialize invetory due to missing config manager");
        }
    }

    private void OnEnable()
    {
        if (inGameEventManager == null)
        {
            inGameEventManager = InGameEventManager.Instance;
        }
        if (inGameEventManager != null)
        {
            inGameEventManager.OpenInventoryEvent.AddListener(ShowInventory);
        }
        if (user != null)
        {
            
            user = User.Instance;
        }
    }

    private void OnDisable()
    {
        if (inGameEventManager != null)
        {
            inGameEventManager.OpenInventoryEvent.RemoveListener(ShowInventory);
        }
    }

    private void ShowInventory()
    {
        gameObject.SetActive(true);
    }

    private void InitalizeInventory(List<InventorySlot> loadedData = null)
    {
        if (loadedData != null)
        {
            inventorySlots = loadedData;
        }
        else
        {
            inventorySlots = new();
        }
        int usedInventorySlotCount = inventorySlots.Count;
        for (int i = 0; i < slotCtrls.Length; i++)
        {
            if (i < usedInventorySlotCount)
            {
                slotCtrls[i].UpdateInventorySlot(inventorySlots[i]);
            }
            else
            {
                slotCtrls[i].UpdateInventorySlot(new InventorySlot());
            }
        }
    }

    private void UpdateSlotLockedState()
    { 
        for (int i = 0; i <slotCtrls.Length; i++)
        {
            slotCtrls[i].SetUnlocked(i < unlockedSlotCount);
        }
    }

    private bool TryPushItem(Item item, int amount)
    {
        return true;
    }

}

public struct InventorySlot
{
    public Item ItemConfig;
    public int Count;
    public bool IsSettedShortCut;
    public string ShortCutKey;
}