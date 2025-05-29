using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillsSO", menuName = "ScriptableObjects/SkillsSO", order = 0)]
public class SkillsSO : ScriptableObject
{
    public string SkillName;

    public int Damage;
    public float Cooldown;
    public int ProjectileNumber;
    public float ProjectileSpeed;
    public float Size;
    public float Duration;
    public float KnockbackForce;

    public BaseSkill Prefab;
}
