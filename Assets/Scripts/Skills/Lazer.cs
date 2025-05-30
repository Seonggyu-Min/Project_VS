using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : BaseSkill
{
    [SerializeField] private ParticleSystem _particleSystem;

    protected override void OnEnable()
    {
        base.OnEnable();
        _particleSystem.Clear();
        _particleSystem.Play();
    }

    protected override void InitSkillSize()
    {
        // 사이즈 설정 취소용 빈 메서드 호출
    }

    protected override void SetDestroyTime()
    {
        _destoryTimer += Time.deltaTime;

        if (_destoryTimer >= 1.4f) // 파티클의 Playback Time이 1.4초라서 _duration 사용 안함
        {
            ReturnPool();
        }
    }

    public override void InitSkillPosition()
    {
        Vector2 spawnVector = _playerTransform.position;

        float randomXOffset = Random.Range(-8f, 8f);

        spawnVector.x += randomXOffset;
        spawnVector.y -= 8f; // 파티클이 아래 기준이라 아래에서 생성

        transform.position = spawnVector;
    }
}
