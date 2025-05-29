using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCardsSO", menuName = "ScriptableObjects/UpgradeCardsSO", order = 0)]
public class UpgradeCardsSO : ScriptableObject, ICard
{
    public CardUpgradeType UpgradeType;
    public string SkillName;

    // 누적합 방식으로 적용되는 데이터, 0.n 으로 범위 조절
    [Header("Skill Upgrade Data")]
    public float DamageMultiplier = 0f;
    public float CooldownMultiplier = 0f;
    public float ProjectileSpeedMultiplier = 0f;
    public float ProjectileNumberMultiplier = 0f;
    public float SizeMultiplier = 0f;
    public float DurationMultiplier = 0f;
    public float KnockbackForceMultiplier = 0f;

    [Header("Player Stat Upgrade Data")]
    public float HealthMultiplier = 0f;
    public float HealthRegenMultiplier = 0f;
    public float ArmorMultiplier = 0f;
    public float MoveSpeedMultiplier = 0f;
    public float GracePeriodMultiplier = 0f;
    public float ExpMultiplier = 0f;
    public float GoldMultiplier = 0f;
    public float MagnetRangeMultiplier = 0f;
    public float LuckMultiplier = 0f;
    public int ReviveMultiplier = 0;


    string ICard.SkillName => SkillName;

    public void ApplyUpgradeCard()
    {
        switch (UpgradeType)
        {
            case CardUpgradeType.SkillUpgrade:
                SkillManager.Instance.ApplyCardUpgrade(this);
                break;
            case CardUpgradeType.StatUpgrade:
                PlayerStatManager.Instance.ApplyCardUpgrade(this);
                break;
            default:
                Debug.LogWarning("Unknown Upgrade Type");
                break;
        }
    }
}

public enum CardUpgradeType
{
    SkillUpgrade = 0,
    StatUpgrade
}
