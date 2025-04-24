using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    // temp use for fast local dev
    // TODO addressable
    private static SpriteManager _instance;
    public static SpriteManager Instance => _instance;

    [SerializeField] private Sprite[] itemSprite;
    [SerializeField] private Sprite[] itemThumbnails;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            if (!_instance)
            {
                _instance = FindAnyObjectByType<SpriteManager>();
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
