using System;
using UnityEngine;
using TMPro;

public class ScoreTextCtrl : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private InGameEventManager inGameEventManager;

    private void OnEnable()
    {
        inGameEventManager = InGameEventManager.Instance;
        if (!inGameEventManager)
        {
            throw new Exception("InGameEventManager is missig");
        }
        inGameEventManager.OnScoreChangeEvent.AddListener(SetScoreText);
    }

    private void OnDisable()
    {
        if (inGameEventManager)
        {
            inGameEventManager.OnScoreChangeEvent.RemoveListener(SetScoreText);
        }
    }

    private void Start()
    {
        var user = User.Instance;
        if (!user)
        {
            throw new Exception("User is missig");
        }
        SetScoreText(user.Score);
    }

    private void SetScoreText(int score)
    {
        if (!scoreText) return;
        string oldValue = scoreText.text;
        string newValue = score.ToString();
        if (oldValue != newValue)
        {
            scoreText.text = newValue;
        }
    }
}
