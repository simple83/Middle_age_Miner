using UnityEngine;
using TMPro;

public class ReceiptDetailItemCtrl : MonoBehaviour
{
    [SerializeField] private TMP_Text detailText;
    [SerializeField] private TMP_Text priceText;

    public void SetItem(string productName, int amount, int price)
    {
        if (detailText!= null)
        {
            detailText.text = $"{productName} X {amount}";
        }
        if (priceText!= null)
        {
            priceText.text = price.ToString();
        }
    }
}
