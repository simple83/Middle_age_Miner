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
                if (oldSlot.Count == 0 || oldSlot.ItemConfig.id == item.id) // 비어있거나 같은 아이템인 경우,
                {
                    newSlot.ItemConfig = item;
                    if (oldSlot.Count + remainder <= stackableCount) // 모두 저장 가능 한 경우,
                    {
                        newSlot.Count = oldSlot.Count + remainder;
                        remainder = 0;
                    }
                    else if (oldSlot.Count < stackableCount) //일부 저장 되는 경우,
                    {
                        newSlot.Count = stackableCount;
                        remainder -= (stackableCount - oldSlot.Count);
                    }
                    else // 하나도 저장을 못하는 경우, 
                    {
                        newSlot.ItemConfig = oldSlot.ItemConfig;
                        newSlot.Count = oldSlot.Count;
                    }
                }
                else // 다른 아이템을 만난 경우,
                {
                    newSlot.ItemConfig = oldSlot.ItemConfig;
                    newSlot.Count = oldSlot.Count;
                }
            }
            else // 모두 집어 넣은 후에는 기존 리스트의 값을 할당
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

        // 현재 인벤토리에서 해당 아이템의 총 수량 계산
        int totalAvailable = 0;
        for (int i = 0; i < unlockedSlotCount; i++)
        {
            if (i >= inventorySlots.Count) break; // 안전 체크
            InventorySlot slot = inventorySlots[i];
            if (slot.ItemConfig.id == item.id)
            {
                totalAvailable += slot.Count;
            }
        }

        // 수량 부족 시 실패 처리
        if (totalAvailable < amount)
        {
            // 인벤토리에 충분한 아이템이 없으므로 아무 것도 변경하지 않고 false 반환
            return false;
        }

        // 아이템 차감 로직 (역순으로 처리)
        int remaining = amount;
        for (int i = unlockedSlotCount - 1; i >= 0 && remaining > 0; i--)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.ItemConfig.id == item.id && slot.Count > 0)
            {
                if (slot.Count > remaining)
                {
                    // 해당 슬롯에서 필요한 만큼만 차감
                    slot.Count -= remaining;
                    remaining = 0;
                    // 변경된 구조체 다시 리스트에 반영
                    inventorySlots[i] = slot;
                }
                else
                {
                    // 슬롯 전체를 소비 (remaining에서 감소)
                    remaining -= slot.Count;
                    // 슬롯을 비움
                    slot.ItemConfig.id = -1;
                    slot.Count = 0;
                    inventorySlots[i] = slot;
                }
            }
        }

        // 모든 요청 수량을 소모하였다면 인벤토리 갱신
        if (remaining == 0)
        {
            UpdateInventorySlots();
            return true;
        }
        else
        {
            // (논리상 이 경우는 발생하지 않음: 사전에 충분한 수량을 확인했기 때문)
            return false;
        }
    }

}

public struct InventorySlot
{
    public Item ItemConfig;
    public int Count;
}