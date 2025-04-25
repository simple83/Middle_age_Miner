using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class TestStarter : MonoBehaviour
{
    private SceneFlowManager flowManager;

    void Start()
    {
        LoadScenesForTest().Forget(); // UniTaskVoid 비동기 함수 호출
    }

    private async UniTaskVoid LoadScenesForTest()
    {
        await SceneManager.LoadSceneAsync("staticScene", LoadSceneMode.Additive);

        await UniTask.WaitUntil(() => SceneFlowManager.Instance != null);

        flowManager = SceneFlowManager.Instance;

        if (flowManager == null)
        {
            throw new System.Exception("SceneFlowManager 인스턴스를 찾을 수 없습니다.");
        }

        flowManager.RequestSceneChange("PlayScene", "startScene");
    }
}
