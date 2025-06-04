using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : BaseSkill
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Rigidbody2D _rb;

    // Axe 스킬은 duration, projectileSpeed의 영향 없음
    [SerializeField] float _impulseForce;
    [SerializeField] float _destroyTimeInterval;

    protected override void OnEnable()
    {
        base.OnEnable();
        AddForce();
    }


    public override void InitSkillPosition()
    {
        // 플레이어 머리 위에서 스킬 생성
        Vector2 spawnVector = _playerTransform.position;
        spawnVector.y += 2f;
        transform.position = spawnVector;

        // 스프라이트를 랜덤으로 뒤집어서 생성
        int randomizer = Random.Range(0, 2);

        if (randomizer == 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    private void AddForce()
    {
        // 플레이어의 머리 위 60도 범위에서 스킬의 힘을 주는 각도 랜덤화
        float randomAngle = Random.Range(60f, 120f);

        float angleRad = randomAngle * Mathf.Deg2Rad;
        Vector2 randomDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        _rb.AddForce(randomDirection * _impulseForce, ForceMode2D.Impulse);
    }

    protected override void SetDestroyTime()
    {
        _destoryTimer += Time.deltaTime;

        // 파괴 시간 간격이 지나면 풀로 반환
        if (_destoryTimer >= _destroyTimeInterval)
        {
            ReturnPool();
        }
    }
}
