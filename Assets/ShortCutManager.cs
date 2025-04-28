using UnityEngine;

public class ShortCutManager : MonoBehaviour
{
    private KeyCode[] ShortCutKeys = { KeyCode.I, KeyCode.Q, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G };
    [SerializeField] InGameEventManager inGameEventManager;

    private bool isInventoryOpened = false;
    private bool isShopOpened = false;

    void Update()
    {
        foreach (KeyCode code in ShortCutKeys)
        {
            if (Input.GetKeyDown(code))
            {
                switch (code)
                {
                    case KeyCode.I:
                        Debug.Log($"I key down");
                        OpenInventory();
                        break;
                    case KeyCode.Q:
                        Debug.Log($"Q key down");
                        CloseInventory();
                        CloseShop();
                        break;
                    case KeyCode.A:
                        SellAllMinerals();
                        UseRegisteredItem(code);
                        break;
                    case KeyCode.S:
                    case KeyCode.D:
                    case KeyCode.F:
                    case KeyCode.G:
                        UseRegisteredItem(code);
                        break;
                }
            }
        }
    }

    private void OnEnable()
    {
        if (inGameEventManager != null)
        {
            inGameEventManager.OpenShopEvent.AddListener(OnShopOpened);
        }
    }

    private void OnDisable()
    {
        if (inGameEventManager != null)
        {
            inGameEventManager.OpenShopEvent.RemoveListener(OnShopOpened);
        }
    }

    private void UseRegisteredItem(KeyCode code)
    {
        if (isInventoryOpened || isShopOpened) return; // �κ��丮�� ���� �ְų� ������ ���� ���� ���� �������� ������� ����
        // �ʿ� �� ���� ���� ���� ������� �ʴ� �ɷ� ����
        Debug.Log($"{code} key down");
    }

    private void OnShopOpened()
    {
        isShopOpened = true;
    }

    private void OpenInventory()
    {
        if (inGameEventManager)
        {
            inGameEventManager.OpenInventoryEvent.Invoke();
        }
        else
        {
            if (InGameEventManager.Instance)
            {
                InGameEventManager.Instance.OpenInventoryEvent.Invoke();
            }
        }
        isInventoryOpened = true;
    }

    private void CloseInventory()
    {
        if (!isInventoryOpened) return;
        if (inGameEventManager)
        {
            inGameEventManager.CloseInventoryEvent.Invoke();
        }
        else
        {
            if (InGameEventManager.Instance)
            {
                InGameEventManager.Instance.CloseInventoryEvent.Invoke();
            }
        }
    }

    private void CloseShop()
    {
        if (!isShopOpened) return;
        if (inGameEventManager)
        {
            inGameEventManager.CloseShopEvent.Invoke();
        }
        else
        {
            if (InGameEventManager.Instance)
            {
                InGameEventManager.Instance.CloseShopEvent.Invoke();
            }
        }
    }

    private void SellAllMinerals()
    {
        if (!isShopOpened) return;
        if (inGameEventManager)
        {
            inGameEventManager.CloseShopEvent.Invoke();
        }
        else
        {
            if (InGameEventManager.Instance)
            {
                InGameEventManager.Instance.CloseShopEvent.Invoke();
            }
        }
    }
}
