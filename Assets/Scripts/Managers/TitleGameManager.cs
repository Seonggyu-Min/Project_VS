using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGameManager : Singleton<TitleGameManager>
{
    public AudioManager AudioManager { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SingletonInit();
        AudioManager = GetComponentInChildren<AudioManager>();
    }
}
