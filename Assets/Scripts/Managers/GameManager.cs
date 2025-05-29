using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public SpawnManager SpawnManager { get; private set; }
    public SkillManager SkillManager { get; private set; }
    public SkillPoolManager SkillPoolManager { get; private set; }


    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        SingletonInit();
        SpawnManager = GetComponentInChildren<SpawnManager>();
        SkillManager = GetComponentInChildren<SkillManager>();
        SkillPoolManager = GetComponentInChildren<SkillPoolManager>();
    }
}
