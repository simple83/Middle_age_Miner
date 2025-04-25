using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class TestStarter : MonoBehaviour
{
    private SceneFlowManager flowManager;

    void Start()
    {
        LoadScenesForTest().Forget(); // UniTaskVoid �񵿱� �Լ� ȣ��
    }

    private async UniTaskVoid LoadScenesForTest()
    {
        await SceneManager.LoadSceneAsync("staticScene", LoadSceneMode.Additive);

        await UniTask.WaitUntil(() => SceneFlowManager.Instance != null);

        flowManager = SceneFlowManager.Instance;

        if (flowManager == null)
        {
            throw new System.Exception("SceneFlowManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }

        flowManager.RequestSceneChange("PlayScene", "startScene");
    }
}
