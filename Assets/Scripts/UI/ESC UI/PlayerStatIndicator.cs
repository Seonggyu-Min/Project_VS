using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PlayerStatIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _statText;
    [SerializeField] private PlayerStatManager _playerStatManager;
    private StringBuilder _sb = new();

    private void OnEnable()
    {
        RenewStat();
    }
    
    private void RenewStat()
    {
        _sb.Clear();
        _sb.AppendLine($"최대 체력: {_playerStatManager.MaxHealth.Value}");
        _sb.AppendLine($"5초당 체력 재생: {_playerStatManager.HealthRegen}");
        _sb.AppendLine($"방어력: {_playerStatManager.Armor}");
        _sb.AppendLine($"이동 속도: {_playerStatManager.MoveSpeed.Value}");
        _sb.AppendLine($"무적 시간: {_playerStatManager.GracePeriod}초");
        _sb.AppendLine($"경험치 배율: {_playerStatManager.ExpMultiplier}");
        _sb.AppendLine($"골드 배율: {_playerStatManager.GoldMultiplier}");
        _sb.AppendLine($"자석 범위 배율: {_playerStatManager.MagnetRangeMultiplier}");
        _sb.AppendLine($"행운 배율: {_playerStatManager.LuckMultiplier}");
        
        //_sb.AppendLine($"부활 횟수: {_playerStatManager.ReviveMultiplier}");
        _sb.AppendLine($"현재 레벨: {_playerStatManager.CurrentLevel.Value}");
        _sb.AppendLine($"다음 레벨업에 필요한 경험치: {_playerStatManager.CurrentExp.Value} / {_playerStatManager.RequiredExpForNextLevel}");

        _statText.text = _sb.ToString();
    }
}
