using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private static AudioPlayer _instance;
    public static AudioPlayer Instance => _instance;

    [SerializeField] private AudioClip[] bgms;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            if (!_instance)
            {
                _instance = FindAnyObjectByType<AudioPlayer>();
            }
            if (!_instance)
            {
                throw new Exception("missing audio player object!");
            }

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
