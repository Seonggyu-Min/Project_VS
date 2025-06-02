using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPbarBehaviour : MonoBehaviour
{
    [SerializeField] private Image _expBar;
    [SerializeField] private PlayerStatManager _playerStat;
    [SerializeField] private TMP_Text _expText;
    private StringBuilder _sb = new();

    private void Start()
    {
        RenewEXPBar();
    }

    public void RenewEXPBar()
    {
        if (_playerStat.CurrentExp.Value >= 0)
        {
            _expBar.fillAmount = (float)_playerStat.CurrentExp.Value / (float)_playerStat.RequiredExpForNextLevel;
        }
        else
        {
            Debug.LogWarning($"현재 경험치 {_playerStat.CurrentExp}, 경험치바 업데이트 실패");
        }
        _sb.Clear();
        _sb.AppendLine($"EXP: {_playerStat.CurrentExp.Value} / {_playerStat.RequiredExpForNextLevel} ({(_playerStat.CurrentExp.Value / (float)_playerStat.RequiredExpForNextLevel * 100):F1}%)");
        _expText.text = _sb.ToString();
    }
}
