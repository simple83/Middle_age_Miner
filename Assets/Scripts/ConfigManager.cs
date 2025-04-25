using System;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    private static ConfigManager _instance;
    public static ConfigManager Instance => _instance;

    public float GameSpeed = 1.0f;
    public int BaseInventorySlotCount = 15;
    [SerializeField] private Status baseStatus;
    [SerializeField] private Equipment[] equipmentConfigs;
    [SerializeField] private Product[] productConfigs;
    [SerializeField] private Item[] itemConfigs;
    [SerializeField] private DropItem[] dropItemConfigs;
    [SerializeField] private Monster[] monsterConfigs;


    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            if (!_instance)
            {
                _instance = FindAnyObjectByType<ConfigManager>();
            }
            if (!_instance)
            {
                throw new Exception("missing config object!");
            }

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}

[Serializable]
public struct Status
{
    public int HP;
    public int O2;
    public int Power;
    public int Speed;
    public int Defense;
}

[Serializable]
public struct Equipment
{
    public string name;
    public int type; //0: helmet, 1:drill, 2:boots
    public EquipmentItem[] equipmentItems;
}

[Serializable]
public struct EquipmentItem
{
    public string name;
    public int id;
    public int thumbnailIndex;
    public int spriteIndex;
    public int addHp;
    public int addO2;
    public int addPower;
    public int addSpeed;
    public int addDefense;
}

[Serializable]
public struct Product
{
    public string name;
    public int id;
    public string desc;
    public int cost;
    public int order;
    public int thumbnailIndex;
    public int itemId;
    public int itemCount;
    public int purchaseLimitPerStage;
    public int unlockedStage;
}

[Serializable]
public struct Item
{
    public string name;
    public int id;
    public int thumbnailIndex;
    public int stackableCount;
    public bool equipmentable;
    public int equipmentType;
    public int equipmentId;
    public bool isQuestItem;
    public int questId;
    public bool sellable;
    public int price;
    public bool consumable;
    public ConsumptionEffect consumptionEffect;
}

[Serializable]
public struct ConsumptionEffect
{
    public int restoreHp;
    public int restoreO2;
    public bool isSpawnObject;
    public int spawnObjcetId;
    public int spawnObjcetPosition; // 0: on tile, 1: below tile
}

[Serializable]
public struct DropItem
{
    public int id;
    public int score;
    public int itemId;
}

[Serializable]
public struct SpawnObject
{
    public int id;
    public GameObject prefab;
}


[Serializable]
public struct Monster
{
    public string name;
    public int id;
    public int hp;
    public int power;
    public int score;
    public bool isBoosMonster;
    public float dropProbability; // 아이템 드롭 확률
    public int[] dropItems; // 아이템 드롭 
    public int[] dropItemProbabilities; // 아이템 별 드롭 확률
}