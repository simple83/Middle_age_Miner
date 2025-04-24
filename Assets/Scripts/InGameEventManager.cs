using System;
using UnityEngine;
using UnityEngine.Events;

public class InGameEventManager : MonoBehaviour
{
    private static InGameEventManager _instance;
    public static InGameEventManager Instance => _instance;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            if (!_instance)
            {
                _instance = FindAnyObjectByType<InGameEventManager>();
            }
            if (!_instance)
            {
                throw new Exception("missing sfx player object!");
            }

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
