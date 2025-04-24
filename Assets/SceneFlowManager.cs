using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneFlowManager : MonoBehaviour
{
    private static SceneFlowManager _instance;
    public static SceneFlowManager Instance => _instance;

    // 현재 로드된 동적 씬 리스트 (StaticScene 제외)
    private List<string> _dynamicScenes = new();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<SceneFlowManager>();
            }
            if (_instance)
            { 
                // 에러 던지고 싶은데
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 동적으로 열릴 씬 목록을 요청받아 교체함
    /// </summary>
    public void RequestSceneChange(string newScene, string oldScene = "")
    {
        StartCoroutine(SwitchScenes(newScene, oldScene));
    }

    private IEnumerator SwitchScenes(string newScenes, string oldScene)
    {
        // 명시적으로 요청한 지난 씬 언로드
        if (oldScene != "")
        {
            yield return SceneManager.UnloadSceneAsync(oldScene);
        }

        // 현재 열린 씬들 언로드
        foreach (var sceneName in _dynamicScenes)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        _dynamicScenes.Clear();
        // 새로운 씬 로드
        yield return LoadAdditiveSceneAsync(newScenes);
        // 해당 씬의 UI가 있으면 같이 로드
        string uiSceneName = GetUISceneName(newScenes);
        if (uiSceneName != "" || uiSceneName != newScenes)
        {
            yield return LoadAdditiveSceneAsync(uiSceneName);
        }
    }

    // TODO: 씬 콘피그 등을 만들어서 관리 
    private string GetUISceneName(string sceneName)
    {
        // 임시로 UI를 붙여서 그냥 반환
        return "UI" + sceneName;
    }

    private IEnumerator LoadAdditiveSceneAsync(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _dynamicScenes.Add(sceneName);
    }
}
