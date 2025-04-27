using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestSetInventorySlot : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button setButton;
    [SerializeField] private Button resetButton;

    private void OnEnable()
    {
        setButton.onClick.AddListener(Set);
        resetButton.onClick.AddListener(ResetPurchasedInventorySlotCount);
    }

    private void OnDisable()
    {
        setButton.onClick.RemoveAllListeners();
        resetButton.onClick.RemoveAllListeners();
    }

    private void Set()
    {
        int.TryParse(input.text, out int count);
        User.Instance.SetPurchaseInventorySlotCount(count);
    }

    private void ResetPurchasedInventorySlotCount()
    {
        User.Instance.SetPurchaseInventorySlotCount(0);
    }
}
