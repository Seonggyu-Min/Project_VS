using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPbarBehaviour : MonoBehaviour
{
    [SerializeField] private BossBehaviour _bossBehaviour;
    [SerializeField] private Image _hpBar;
    [SerializeField] private TMP_Text _hpText;
    private StringBuilder _sb = new();

    private void Start()
    {
        RenewHPBar();
    }

    public void RenewHPBar()
    {
        if (_bossBehaviour.CurrentHealth.Value >= 0)
        {
            _hpBar.fillAmount = (float)_bossBehaviour.CurrentHealth.Value / (float)_bossBehaviour.MaxHealth;
        }

        else
        {
            Debug.LogWarning($"현재 체력 {_bossBehaviour.CurrentHealth.Value}, 체력바 업데이트 실패");
        }

        _sb.Clear();
        _sb.AppendLine($"HP: {_bossBehaviour.CurrentHealth.Value} / {_bossBehaviour.MaxHealth} ({(_bossBehaviour.CurrentHealth.Value / _bossBehaviour.MaxHealth * 100):F1}%)");
        _hpText.text = _sb.ToString();
    }
}
