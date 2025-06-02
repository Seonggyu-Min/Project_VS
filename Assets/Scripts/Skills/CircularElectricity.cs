using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularElectricity : BaseSkill
{
    [SerializeField] private List<IDamageable> _targetsList = new();
    [SerializeField] private float _damageInterval = 0.5f; // 공격 주기
    [SerializeField] private float _damageTimer = 0f; // 누적 시간


    protected override void Update()
    {
        // SetDestroyTime() 하지 않게 base 호출 안함
        SetDamageTime();
    }

    private void FixedUpdate()
    {
        Move();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HandleTriggerExit(collision);
    }


    public override void InitSkillPosition()
    {
        Vector2 spawnPos = _playerTransform.position;
        spawnPos.y += 1f; //플레이어의 중앙에 위치

        transform.position = spawnPos;
    }

    private void Move()
    {
        Vector2 spawnPos = _playerTransform.position;
        spawnPos.y += 1f; //플레이어의 중앙에 위치

        transform.position = spawnPos;
    }

    private void SetDamageTime()
    {
        _damageTimer += Time.deltaTime;

        if (_damageTimer >= _damageInterval)
        {
            _damageTimer = 0f;
            Attack();
        }
    }

    private void Attack()
    {
        for (int i = _targetsList.Count - 1; i >= 0; i--)
        {
            MonoBehaviour mb = _targetsList[i] as MonoBehaviour;

            if (mb == null || !mb.gameObject.activeSelf)
            {
                _targetsList.RemoveAt(i);
                continue;
            }

            _targetsList[i].TakeDamage(_damage);
        }
    }

    private void HandleTriggerEnter(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _monsterLayermask) != 0)
        {
            ReferenceProvider provider = ReferenceRegistry.GetProvider(collision.gameObject);

            if (provider == null)
            {
                Debug.LogWarning($"ReferenceProvider가 {collision.gameObject.name}에 없음");
                return;
            }

            IDamageable damageable = provider.GetAs<IDamageable>();

            if (damageable != null)
            {
                _targetsList.Add(damageable);
            }
            else
            {
                Debug.LogWarning($"IDamageable이 {collision.gameObject.name}에 없음");
            }
        }
    }

    private void HandleTriggerExit(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _monsterLayermask) != 0)
        {
            ReferenceProvider provider = ReferenceRegistry.GetProvider(collision.gameObject);

            if (provider == null)
            {
                Debug.LogWarning($"ReferenceProvider가 {collision.gameObject.name}에 없음");
                return;
            }

            IDamageable damageable = provider.GetAs<IDamageable>();

            if (damageable != null)
            {
                _targetsList.Remove(damageable);
            }
            else
            {
                Debug.LogWarning($"IDamageable이 {collision.gameObject.name}에 없음");
            }
        }
    }
}
