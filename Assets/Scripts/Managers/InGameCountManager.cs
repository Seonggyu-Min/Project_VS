using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCountManager : MonoBehaviour
{
    public int KillCount { get; private set; } = 0;

    public void AddKillCount()
    {
        KillCount++;
    }

    // TODO: 초기화 기능 및 골드로 환산 기능 추가
}
