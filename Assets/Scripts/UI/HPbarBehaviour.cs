using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPbarBehaviour : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private PlayerStatManager _playerStat;
    [SerializeField] private TMP_Text _hpText;
    private StringBuilder _sb = new();

    private void Start()
    {
        RenewHPBar();
    }

    public void RenewHPBar()
    {
        if (_playerStat.CurrentHealth.Value >= 0)
        {
            _hpBar.fillAmount = (float)_playerStat.CurrentHealth.Value / (float)_playerStat.MaxHealth.Value;
        }

        else
        {
            Debug.LogWarning($"현재 체력 {_playerStat.CurrentHealth.Value}, 체력바 업데이트 실패");
        }

        _sb.Clear();
        _sb.AppendLine($"HP: {_playerStat.CurrentHealth.Value} / {_playerStat.MaxHealth.Value} ({(_playerStat.CurrentHealth.Value / _playerStat.MaxHealth.Value * 100):F1}%)");
        _hpText.text = _sb.ToString();
    }
}
