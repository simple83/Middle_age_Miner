using UnityEngine;

public class ShortCutManager : MonoBehaviour
{
    private KeyCode[] ShortCutKeys = { KeyCode.I, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G };
    [SerializeField] InGameEventManager inGameEventManager;

    void Update()
    {
        foreach (KeyCode code in ShortCutKeys)
        {
            if (Input.GetKeyDown(code))
            {
                switch (code)
                {
                    case KeyCode.I:
                        ShowInventory();
                        break;
                    case KeyCode.A:
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

    private void UseRegisteredItem(KeyCode code)
    {
        Debug.Log($"{code} key down");
    }

    void ShowInventory()
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
    }
}
