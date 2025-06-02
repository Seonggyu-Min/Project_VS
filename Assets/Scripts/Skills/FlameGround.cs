using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameGround : BaseSkill
{
    [SerializeField] private List<IDamageable> _targetsList = new();
    [SerializeField] private float _damageInterval = 0.5f; // 공격 주기
    [SerializeField] private float _damageTimer = 0f; // 누적 시간


    protected override void Update()
    {
        base.Update();
        SetDamageTime();
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
        Vector2 spawnVector = _playerTransform.position;

        // TODO: size에 따른 Range 변경
        float xOffset = Random.Range(-3f, 3f);
        float yOffset = Random.Range(-3f, 3f);

        spawnVector.x += xOffset;
        spawnVector.y += yOffset;

        //Debug.Log($"xOffset: {xOffset}, yOffset: {yOffset}");

        transform.position = spawnVector;
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
