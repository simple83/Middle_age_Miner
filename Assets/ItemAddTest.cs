using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemAddTest : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button button;
    [SerializeField] private Button spendButton;
    
    private Item[] itemConfigs;

    private void Start()
    {
        itemConfigs = ConfigManager.Instance.GetAllItemConfigs();
        if (itemConfigs != null)
        {
            SetupOptions();
        }
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClickAddItemButton);
        spendButton.onClick.AddListener(OnClickSpendItemButton);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClickAddItemButton);
        spendButton.onClick.AddListener(OnClickSpendItemButton);
    }

    private void SetupOptions()
    {
        dropdown.ClearOptions();
        List<string> optionNames = new();
        for (int i =0; i < itemConfigs.Length; i++)
        {
            optionNames.Add(itemConfigs[i].name);
        }
        dropdown.AddOptions(optionNames);
    }

    private void OnClickAddItemButton()
    {
        Debug.Log("OnClickAddItemButton");
        int.TryParse(input.text, out int amount);
        if (amount > 0)
        {
            Item? selectItem = null;
            foreach (Item item in itemConfigs)
            {
                if (item.name == dropdown.options[dropdown.value].text)
                {
                    selectItem = item;
                    break;
                }
            }

            if (selectItem.HasValue)
            {
                InGameEventManager.Instance.OnGetItemFromObjectEvent.Invoke(selectItem.Value, amount, null);
            }
        }
    }


    private void OnClickSpendItemButton()
    {
        Debug.Log("OnClickSpendItemButton");
        int.TryParse(input.text, out int amount);
        if (amount > 0)
        {
            Item? selectItem = null;
            foreach (Item item in itemConfigs)
            {
                if (item.name == dropdown.options[dropdown.value].text)
                {
                    selectItem = item;
                    break;
                }
            }

            if (selectItem.HasValue)
            {
                InGameEventManager.Instance.OnUseInventoryItemEvent.Invoke(selectItem.Value, amount);
            }
        }
    }
}
