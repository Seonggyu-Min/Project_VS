using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public InGameCountManager InGameCountManager { get; private set; }
    public WinOrLoseManager WinOrLoseManager { get; private set; }

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        SingletonInit();
        InGameCountManager = GetComponentInChildren<InGameCountManager>();
        WinOrLoseManager = GetComponentInChildren<WinOrLoseManager>();
    }
}
