using UnityEngine;

public class TestButtonCtrl : MonoBehaviour
{
    public void OnButtonClicked()
    {
        User.Instance.AddScore(100);
    }
}
