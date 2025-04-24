using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneFlowManager : MonoBehaviour
{
    private static SceneFlowManager _instance;
    public static SceneFlowManager Instance => _instance;

    // ���� �ε�� ���� �� ����Ʈ (StaticScene ����)
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
                // ���� ������ ������
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �������� ���� �� ����� ��û�޾� ��ü��
    /// </summary>
    public void RequestSceneChange(string newScene, string oldScene = "")
    {
        StartCoroutine(SwitchScenes(newScene, oldScene));
    }

    private IEnumerator SwitchScenes(string newScenes, string oldScene)
    {
        // ��������� ��û�� ���� �� ��ε�
        if (oldScene != "")
        {
            yield return SceneManager.UnloadSceneAsync(oldScene);
        }

        // ���� ���� ���� ��ε�
        foreach (var sceneName in _dynamicScenes)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        _dynamicScenes.Clear();
        // ���ο� �� �ε�
        yield return LoadAdditiveSceneAsync(newScenes);
        // �ش� ���� UI�� ������ ���� �ε�
        string uiSceneName = GetUISceneName(newScenes);
        if (uiSceneName != "" || uiSceneName != newScenes)
        {
            yield return LoadAdditiveSceneAsync(uiSceneName);
        }
    }

    // TODO: �� ���Ǳ� ���� ���� ���� 
    private string GetUISceneName(string sceneName)
    {
        // �ӽ÷� UI�� �ٿ��� �׳� ��ȯ
        return "UI" + sceneName;
    }

    private IEnumerator LoadAdditiveSceneAsync(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _dynamicScenes.Add(sceneName);
    }
}
