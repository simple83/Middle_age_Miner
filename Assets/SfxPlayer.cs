using System;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    private static SfxPlayer _instance;
    public static SfxPlayer Instance => _instance;

    [SerializeField] private AudioClip[] sfxs;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            if (!_instance)
            {
                _instance = FindAnyObjectByType<SfxPlayer>();
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
