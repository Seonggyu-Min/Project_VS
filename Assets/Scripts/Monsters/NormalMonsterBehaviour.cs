using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterBehaviour : BaseMonster
{

    protected override void InitChildMonster()
    {
        _damage = _monstersSO.Damage;
        _maxHealth = _monstersSO.MaxHealth;
        _currentHealth = _maxHealth;
        _dropExp = _monstersSO.DropExp;
        _moveSpeed = _monstersSO.Speed;
    }
}
