using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCountManager : MonoBehaviour
{
    private int _killCount = 0;
    public int KillCount => _killCount;

    private void OnEnable()
    {
        _killCount = 0;
    }

    public void AddKillCount()
    {
        _killCount++;
    }

    // TODO: 초기화 기능 및 골드로 환산 기능 추가
}
