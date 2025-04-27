using System;
using UnityEngine;
using UnityEngine.Events;

public class InGameEventManager : MonoBehaviour
{
    private static InGameEventManager _instance;
    public static InGameEventManager Instance => _instance;

    [HideInInspector] public UnityEvent<int> OnScoreChangeEvent = new();
    [HideInInspector] public UnityEvent<int> OnMoneyChangeEvent = new();
    [HideInInspector] public UnityEvent OpenInventoryEvent = new();
    [HideInInspector] public UnityEvent<Item, int, GameObject> OnGetItemFromObjectEvent = new();
    [HideInInspector] public UnityEvent<Item, int> OnUseInventoryItemEvent = new();

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            if (!_instance)
            {
                _instance = FindAnyObjectByType<InGameEventManager>();
            }
            if (!_instance)
            {
                throw new Exception("missing IngameManager object!");
            }

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
