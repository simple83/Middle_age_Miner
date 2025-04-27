using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCtrl : MonoBehaviour
{
    private InGameEventManager inGameEventManager;
    private User user;

    private int baseUnlockedSlotCount = 15;
    private int unlockedSlotCount => baseUnlockedSlotCount + user.PurchasedInventroySlotCount;
    private List<InventorySlot> inventorySlots = new();
    [SerializeField] private InventorySlotCtrl[] slotCtrls;

    private void Awake()
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
            UpdateSlotLockedState();
        }
        else
        {
            throw new Exception("fail to initialize invetory due to missing config manager");
        }
        inGameEventManager = InGameEventManager.Instance;
        if (inGameEventManager == null)
        {
            throw new Exception("ingamemanager is missing");
        }
        inGameEventManager.OpenInventoryEvent.AddListener(ShowInventory);
        inGameEventManager.OnGetItemFromObjectEvent.AddListener(OnGetItemFromObject);
        inGameEventManager.OnUseInventoryItemEvent.AddListener(OnSpendItem);
        inGameEventManager.OnPurchaseInventoryCountChangeEvent.AddListener(UpdateSlotLockedState);
        gameObject.SetActive(false);
        
        InitalizeInventory();
    }

    private void OnDestroy()
    {
        if (inGameEventManager != null)
        {
            inGameEventManager.OpenInventoryEvent.RemoveListener(ShowInventory);
            inGameEventManager.OnGetItemFromObjectEvent.RemoveListener(OnGetItemFromObject);
            inGameEventManager.OnUseInventoryItemEvent.RemoveListener(OnSpendItem);
            inGameEventManager.OnPurchaseInventoryCountChangeEvent.RemoveListener(UpdateSlotLockedState);
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
        for (int i = inventorySlots.Count; i < unlockedSlotCount; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
        UpdateInventorySlots();
    }

    private void UpdateInventorySlots()
    {
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

    private void OnGetItemFromObject(Item item, int amount, GameObject target)
    {
        Debug.Log($"{item.id} item, amount: {amount} event recived");
        if (TryPushItem(item, amount))
        {
            if (target != null)
            {
                target.SetActive(false);
            }
        }
    } 

    public bool TryPushItem(Item item, int amount)
    {
        List<InventorySlot> newSlots = new();
        int remainder = amount;
        int stackableCount = item.stackableCount;
        for (int i = 0; i < unlockedSlotCount; i++)
        {
            InventorySlot oldSlot = inventorySlots[i];
            InventorySlot newSlot = new();
            if (remainder > 0)
            {
                if (oldSlot.Count == 0 || oldSlot.ItemConfig.id == item.id) // ����ְų� ���� �������� ���,
                {
                    newSlot.ItemConfig = item;
                    if (oldSlot.Count + remainder <= stackableCount) // ��� ���� ���� �� ���,
                    {
                        newSlot.Count = oldSlot.Count + remainder;
                        remainder = 0;
                    }
                    else if (oldSlot.Count < stackableCount) //�Ϻ� ���� �Ǵ� ���,
                    {
                        newSlot.Count = stackableCount;
                        remainder -= (stackableCount - oldSlot.Count);
                    }
                    else // �ϳ��� ������ ���ϴ� ���, 
                    {
                        newSlot.ItemConfig = oldSlot.ItemConfig;
                        newSlot.Count = oldSlot.Count;
                    }
                }
                else // �ٸ� �������� ���� ���,
                {
                    newSlot.ItemConfig = oldSlot.ItemConfig;
                    newSlot.Count = oldSlot.Count;
                }
            }
            else // ��� ���� ���� �Ŀ��� ���� ����Ʈ�� ���� �Ҵ�
            {
                newSlot.ItemConfig = oldSlot.ItemConfig;
                newSlot.Count = oldSlot.Count;
            }
            newSlots.Add(newSlot);
        }
        if (remainder == 0)
        {
            inventorySlots = newSlots;
            UpdateInventorySlots();
            return true;
        }
        return false;
    }

    private void OnSpendItem(Item item, int amount)
    {
        Debug.Log($"{item.id} item, amount: {amount} event recived");
        if (TrySpendItem(item, amount))
        {
            Debug.Log("use item success");
        }
        else
        {
            Debug.Log("fail to use item");
        }
    }

    public bool TrySpendItem(Item item, int amount)
    {
        if (amount <= 0)
            return false;

        // ���� �κ��丮���� �ش� �������� �� ���� ���
        int totalAvailable = 0;
        for (int i = 0; i < unlockedSlotCount; i++)
        {
            if (i >= inventorySlots.Count) break; // ���� üũ
            InventorySlot slot = inventorySlots[i];
            if (slot.ItemConfig.id == item.id)
            {
                totalAvailable += slot.Count;
            }
        }

        // ���� ���� �� ���� ó��
        if (totalAvailable < amount)
        {
            // �κ��丮�� ����� �������� �����Ƿ� �ƹ� �͵� �������� �ʰ� false ��ȯ
            return false;
        }

        // ������ ���� ���� (�������� ó��)
        int remaining = amount;
        for (int i = unlockedSlotCount - 1; i >= 0 && remaining > 0; i--)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.ItemConfig.id == item.id && slot.Count > 0)
            {
                if (slot.Count > remaining)
                {
                    // �ش� ���Կ��� �ʿ��� ��ŭ�� ����
                    slot.Count -= remaining;
                    remaining = 0;
                    // ����� ����ü �ٽ� ����Ʈ�� �ݿ�
                    inventorySlots[i] = slot;
                }
                else
                {
                    // ���� ��ü�� �Һ� (remaining���� ����)
                    remaining -= slot.Count;
                    // ������ ���
                    slot.ItemConfig.id = -1;
                    slot.Count = 0;
                    inventorySlots[i] = slot;
                }
            }
        }

        // ��� ��û ������ �Ҹ��Ͽ��ٸ� �κ��丮 ����
        if (remaining == 0)
        {
            UpdateInventorySlots();
            return true;
        }
        else
        {
            // (���� �� ���� �߻����� ����: ������ ����� ������ Ȯ���߱� ����)
            return false;
        }
    }

}

public struct InventorySlot
{
    public Item ItemConfig;
    public int Count;
}