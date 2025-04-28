using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopCtrl : MonoBehaviour
{
    [SerializeField] private InventoryCtrl inventory;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Transform productContainer;
    [SerializeField] private GameObject prodcutPrefab;

    private Dictionary<int, ProductCtrl> productCtrls = new();
    private Dictionary<int, int> purchasedCounter = new();
    private InGameEventManager inGameEventManager;

    private void Awake()
    {
        inGameEventManager = InGameEventManager.Instance;
        if (inGameEventManager == null)
        {
            throw new Exception("ingameevnetmanager is missing");
        }
        shopUI.SetActive(false);
    }

    private void Start()
    {
        var configs = ConfigManager.Instance.GetProductConfigs();
        if (configs.Length > 0)
        {
            InitProducts(configs);
        }
    }

    private void OnEnable()
    {
        if (inGameEventManager != null)
        {
            inGameEventManager.OnPurchaeProductEvent.AddListener(OnPurchaseProduct);
        }
    }

    private void OnDisable()
    {
        if (inGameEventManager != null)
        {
            inGameEventManager.OnPurchaeProductEvent.RemoveListener(OnPurchaseProduct);
        }
    }

    private void OnPurchaseProduct(Product product)
    {
        var currentPurchaseCount = purchasedCounter[product.id];
        SetPurchaseCount(product.id, currentPurchaseCount + 1);
    }

    private void InitProducts(Product[] configs)
    {
        foreach (var config in configs)
        {
            var go = Instantiate(prodcutPrefab, productContainer, false);
            var ctrl = go.GetComponent<ProductCtrl>();
            ctrl.SetProduct(config, inventory);
            productCtrls.Add(config.id, ctrl);
            SetPurchaseCount(config.id, 0);
        }
    }

    private void SetPurchaseCount(int productId, int count)
    {
        purchasedCounter[productId] = count;
        productCtrls.TryGetValue(productId, out ProductCtrl ctrl);
        if (ctrl == null) return;
        ctrl.SetPurchasedCount(count);
    }
}
