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

    [SerializeField] private InGameEventManager inGameEventManager;

    private void OnEnable()
    {
        if (inGameEventManager == null)
        {
            inGameEventManager = InGameEventManager.Instance;
            if (inGameEventManager == null)
            {
                throw new Exception("Ingamemanger is missing");
            }
        }
    }

    public void AddScore(int score)
    {
        Score += score;
        inGameEventManager.OnScoreChangeEvent.Invoke(Score);
    }

    public void EarnMoney(int amount)
    {
        Money += amount;
        inGameEventManager.OnScoreChangeEvent.Invoke(Money);
    }

    public bool SpendMoney(int amount)
    {
        if (Money < amount)
        {
            return false;
        }
        Money -= amount;
        inGameEventManager.OnScoreChangeEvent.Invoke(Money);
        return true;
    }

    public void SetPurchaseInventorySlotCount(int slotCount)
    {
        PurchasedInventroySlotCount = slotCount;
        inGameEventManager.OnPurchaseInventoryCountChangeEvent.Invoke();
    }
}

public class PlayerStatus
{
    private Status currentStatus = new();
    public int CurrentDepth { get; private set; }

    public PlayerStatus(Status initStatus)
    {
        currentStatus.HP = initStatus.HP;
        currentStatus.O2 = initStatus.O2;
        currentStatus.Power = initStatus.Power;
        currentStatus.Speed = initStatus.Speed;
        currentStatus.Defense = initStatus.Defense;
    }
}

