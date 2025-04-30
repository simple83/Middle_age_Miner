using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class ReceiptCtrl : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject item;
    [SerializeField] private TMP_Text totalText;
    private List<SellingItem> sellingItemList;
    private InGameEventManager inGameEventManager;

    private void Awake()
    {
        inGameEventManager = InGameEventManager.Instance;
        if (inGameEventManager == null )
        {
            throw new Exception("inGameManager is missing");
        }
    }

    public void ShowReceipt(List<SellingItem> items)
    {
        sellingItemList = items;
        SetReceptItems();
        SetTexts();
        gameObject.SetActive(true);
    }

    private void SetReceptItems()
    {
        for (int i = 0; i < sellingItemList.Count; i++)
        {
            var sellingItem = sellingItemList[i];
            var name = sellingItem.itemConfig.name;
            var quantity = sellingItem.amount;
            var price = sellingItem.itemConfig.price * quantity;
            Instantiate(item, itemParent, false).GetComponent<ReceiptDetailItemCtrl>().SetItem(name, quantity, price);
        }
    }

    private void SetTexts()
    {
        int totalPrice = 0;

        for (int i = 0; i < sellingItemList.Count; i++)
        {
            var sellingItem = sellingItemList[i];
            var quantity = sellingItem.amount;
            totalPrice += sellingItem.itemConfig.price * quantity;
        }

        totalText.text = $"total price : {totalPrice}";
    }

    private void OnEnable()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(() => inGameEventManager.OnInventoryItemSoldEvent.Invoke(sellingItemList));
        }
        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }

    private void OnDisable()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
        }
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
        }
    }


}

public struct SellingItem
{
    public int amount;
    public Item itemConfig;
}