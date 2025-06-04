using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinOrLoseManager : MonoBehaviour
{
    private bool _isWin;
    public bool IsWin => _isWin;

    private float _playedTime;
    public float PlayedTime => _playedTime;

    public void SetWin()
    {
        _playedTime = WeaterManager.Instance.PlayTime;
        _isWin = true;
        SceneManager.LoadScene(2);
    }

    public void SetLose()
    {
        _playedTime = WeaterManager.Instance.PlayTime;
        _isWin = false;
        SceneManager.LoadScene(2);
    }
}
