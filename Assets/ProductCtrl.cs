using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductCtrl : MonoBehaviour
{
    [SerializeField] private Image itemThumbnail;
    [SerializeField] private Button button;
    [SerializeField] private GameObject soldOut;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;

    private Product? productConfig = null;
    private InventoryCtrl inventory;

    private void OnEnable()
    {
        if (button!= null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnDisable()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnButtonClick()
    {
        var user = User.Instance;
        if (user == null) return;
        if (InGameEventManager.Instance == null) return;
        if (!productConfig.HasValue) return;
        var product = productConfig.Value;
        var cost = product.cost;
        if (!inventory.TryPushItem(product)) // 인벤토리에 아이템이 들어가는지 체크
        {
            Debug.Log("inventory is full");
            return;
        }
        if (!user.SpendMoney(cost)) // 돈이 있는지 체크
        {
            Debug.Log("lack of money");
            return;
        }
        else
        {
            Debug.Log("success to purchase");
            InGameEventManager.Instance.OnPurchaeProductEvent.Invoke(product);
        }
    }

    public void SetProduct(Product product, InventoryCtrl inventory)
    {
        productConfig = product;
        this.inventory = inventory;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!productConfig.HasValue) return;
        var product = productConfig.Value;
        var itemConfig = ConfigManager.Instance.GetItemConfig(product.itemId);
        if (itemThumbnail!= null)
        {
            itemThumbnail.sprite = SpriteManager.Instance.GetItemThumbnail(itemConfig.thumbnailIndex);
        }
        if (itemNameText != null)
        {
            itemNameText.text = product.name;
        }
        if (priceText != null)
        {
            priceText.text = product.cost.ToString();
        }
    }

    public void SetPurchasedCount(int count)
    {
        if (!productConfig.HasValue) return;
        if (productConfig.Value.purchaseLimitPerStage <= 0) return;
        soldOut.SetActive(productConfig.Value.purchaseLimitPerStage <= count);
    }
}
