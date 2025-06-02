using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SkillUpgradeIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _upgradesText;
    [SerializeField] private SkillManager _skillManager;
    private StringBuilder _sb = new();

    private void OnEnable()
    {
        RenewUpgrades();
    }

    private void RenewUpgrades()
    {
        _sb.Clear();

        foreach (var skill in _skillManager.SkillDict)
        {
            _sb.AppendLine($"스킬 {skill.Value.SkillName} 업그레이드 현황");

            _sb.AppendLine($"기본 데미지: {skill.Value.Data.Damage}");
            _sb.AppendLine($"증가된 데미지 배율: X {skill.Value.DamageMultiplier}");
            _sb.AppendLine($"최종 데미지: {Mathf.RoundToInt(skill.Value.Data.Damage * skill.Value.DamageMultiplier)}");

            _sb.AppendLine($"기본 쿨타임: {skill.Value.Data.Cooldown}");
            _sb.AppendLine($"증가된 쿨타임 배율: X {skill.Value.CooldownMultiplier}");
            _sb.AppendLine($"최종 쿨타임: {Mathf.Max(0.1f, skill.Value.Data.Cooldown * skill.Value.CooldownMultiplier):F1}");

            _sb.AppendLine($"기본 투사체 개수: {skill.Value.Data.ProjectileNumber}");
            _sb.AppendLine($"증가된 투사체 개수 배율: X {skill.Value.ProjectileNumberMultiplier}");
            _sb.AppendLine($"최종 투사체 개수: {Mathf.RoundToInt(skill.Value.Data.ProjectileNumber * skill.Value.ProjectileNumberMultiplier)}");

            _sb.AppendLine($"기본 투사체 속도: {skill.Value.Data.ProjectileSpeed}");
            _sb.AppendLine($"증가된 투사체 속도 배율: X {skill.Value.ProjectileSpeedMultiplier}");
            _sb.AppendLine($"최종 투사체 속도: {skill.Value.Data.ProjectileSpeed * skill.Value.ProjectileSpeedMultiplier:F1}");

            _sb.AppendLine($"기본 크기: {skill.Value.Data.Size}");
            _sb.AppendLine($"증가된 크기 배율: X {skill.Value.SizeMultiplier}");
            _sb.AppendLine($"최종 크기: {skill.Value.Data.Size * skill.Value.SizeMultiplier:F1}");

            _sb.AppendLine($"기본 지속시간: {skill.Value.Data.Duration}");
            _sb.AppendLine($"증가된 지속시간 배율: X {skill.Value.DurationMultiplier}");
            _sb.AppendLine($"최종 지속시간: {skill.Value.Data.Duration * skill.Value.DurationMultiplier:F1}");

            _sb.AppendLine($"기본 넉백력: {skill.Value.Data.KnockbackForce}");
            _sb.AppendLine($"증가된 넉백력 배율: X {skill.Value.KnockbackForceMultiplier}");
            _sb.AppendLine($"최종 넉백력: {skill.Value.Data.KnockbackForce * skill.Value.KnockbackForceMultiplier:F1}");

            _sb.AppendLine("--------------------------");
        }

        _upgradesText.text = _sb.ToString();
    }
}
