using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonstersSO", menuName = "ScriptableObjects/MonstersSO", order = 0)]
public class MonstersSO : ScriptableObject
{
    public string MonsterName;
    public int Damage;
    public float Speed;
    public int MaxHealth;
    public int DropExp;
}
