using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class OverallStatText : MonoBehaviour
{
    [SerializeField] private TMP_Text _overallStatText;
    private StringBuilder _statSb = new();

    private void OnEnable()
    {
        RenewStatText();
    }

    public void RenewStatText()
    {
        _statSb.Clear();

        _statSb.AppendLine($"Health: {PlayerStatManager.Instance.MaxHealth}");
        _statSb.AppendLine($"Armor: {PlayerStatManager.Instance.Armor}");
        _statSb.AppendLine($"Health Regen: {PlayerStatManager.Instance.HealthRegen}");
        _statSb.AppendLine($"Move Speed: {PlayerStatManager.Instance.MoveSpeed.Value}");
        _statSb.AppendLine($"Grace Period: {PlayerStatManager.Instance.GracePeriod}");
        _statSb.AppendLine($"Exp Multiplier: {PlayerStatManager.Instance.ExpMultiplier}");
        _statSb.AppendLine($"Gold Multiplier: {PlayerStatManager.Instance.GoldMultiplier}");
        _statSb.AppendLine($"Magnet Range Multiplier: {PlayerStatManager.Instance.MagnetRangeMultiplier}");
        _statSb.AppendLine($"Luck Multiplier: {PlayerStatManager.Instance.LuckMultiplier}");
        _statSb.AppendLine($"Revive Multiplier: {PlayerStatManager.Instance.ReviveMultiplier}");
        _overallStatText.text = _statSb.ToString();
    }
}
