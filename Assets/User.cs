using System;
using UnityEngine;

public class User : MonoBehaviour
{
    private static User _instance;
    public static User Instance => _instance;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            if (!_instance)
            {
                _instance = FindAnyObjectByType<User>();
            }
            if (!_instance)
            {
                throw new Exception("missing sfx player object!");
            }

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public int StageIndex { get; private set; }
    public int Score { get; private set; }
    public int Money { get; private set; }
    public int PurchasedInventroySlotCount { get; private set; }
    public EquipmentItem EquipSlot0 { get; private set; } // type 0
    public EquipmentItem EquipSlot1 { get; private set; } // type 1
    public EquipmentItem EquipSlot2 { get; private set; } // type 2

}

public class HeroStatus
{
    private Status currentStatus = new();
    public int CurrentDepth { get; private set; }

    public HeroStatus(Status initStatus)
    {
        currentStatus.HP = initStatus.HP;
        currentStatus.O2 = initStatus.O2;
        currentStatus.Power = initStatus.Power;
        currentStatus.Speed = initStatus.Speed;
        currentStatus.Defense = initStatus.Defense;
    }
}

// 요놈은 UI에 붙여 버리자.
public class Inventory
{
    private InventorySlot[] slots = null;
    public Inventory(int slotCount)
    {
        slots = new InventorySlot[slotCount];
    }
}

public struct InventorySlot
{
    public Item ItemId;
    public int Count;
    public bool IsSettedShortCut;
    public string ShortCutKey;
}