using UnityEngine;

public class TestButtonCtrl : MonoBehaviour
{
    public void OnButtonClicked()
    {
        User.Instance.AddScore(100);
    }

    public void OpenShop()
    {
        InGameEventManager.Instance.OpenShopEvent.Invoke();
    }
}
