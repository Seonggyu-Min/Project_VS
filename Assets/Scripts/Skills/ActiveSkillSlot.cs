using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class ActiveSkillSlot
{
    public SkillsSO Data { get; private set; }

    private float _damageMultiplier = 1f;
    private float _cooldownMultiplier = 1f;
    private float _projectileNumberMultiplier = 1f;
    private float _projectileSpeedMultiplier = 1f;
    private float _sizeMultiplier = 1f;
    private float _durationMultiplier = 1f;
    private float _knockbackForceMultiplier = 1f;


    public string SkillName => Data.SkillName;
    public float DamageMultiplier => _damageMultiplier;
    public float CooldownMultiplier => _cooldownMultiplier;
    public float ProjectileNumberMultiplier => _projectileNumberMultiplier;
    public float ProjectileSpeedMultiplier => _projectileSpeedMultiplier;
    public float SizeMultiplier => _sizeMultiplier;
    public float DurationMultiplier => _durationMultiplier;
    public float KnockbackForceMultiplier => _knockbackForceMultiplier;




    public float Cooldown => Data.Cooldown;
    private float _cooldownTimer;
    public ObservableProperty<bool> IsReady { get; private set; } = new();

    private Coroutine _cooldownCoroutine;
    private Coroutine _multipleUseCoroutine;
    [SerializeField] private float _multipleUseDelay = 0.2f; // 다중 사용 시 딜레이 시간

    private MonoBehaviour _context;


    public ActiveSkillSlot(SkillsSO data, MonoBehaviour context)
    {
        Data = data;
        _context = context;

        IsReady.Value = true; // 초기 상태는 항상 사용 가능
    }

    public void TryUseSkill(Transform playerTransform, PlayerMove playerMove)
    {
        if (!IsReady.Value) return;

        BaseSkill skill = SkillPoolManager.Instance.GetSkillInstance(Data.Prefab); // 프리팹을 인스턴스화해서 풀링, CircularElectricity는 풀링이 필요없으므로 1개만 생성하도록 예외처리 필요함
        skill.SetPlayerReferences(playerTransform, playerMove);
        skill.InitSkillPosition();

        skill.UpgradeSkill(
            damage: _damageMultiplier,
            cd: _cooldownMultiplier,
            projectileNumber: _projectileNumberMultiplier,
            projectileSpeed: _projectileSpeedMultiplier,
            size: _sizeMultiplier,
            duration: _durationMultiplier,
            knockbackForce: _knockbackForceMultiplier
            );

        if (skill.ProjectileNumber > 1)
        {
            _multipleUseCoroutine = _context.StartCoroutine(MultipleUseRoutine((skill.ProjectileNumber - 1), playerTransform, playerMove));
        }

        _cooldownCoroutine = _context.StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        IsReady.Value = false;
        _cooldownTimer = 0f;

        while (_cooldownTimer < Cooldown)
        {
            _cooldownTimer += Time.deltaTime;
            yield return null;
        }

        IsReady.Value = true;
    }

    private IEnumerator MultipleUseRoutine(int num, Transform playerTransform, PlayerMove playerMove)
    {
        for (int i = 0; i < num; i++)
        {
            yield return new WaitForSeconds(_multipleUseDelay);
            
            BaseSkill skill = SkillPoolManager.Instance.GetSkillInstance(Data.Prefab); // 프리팹을 인스턴스화해서 풀링, CircularElectricity는 풀링이 필요없으므로 1개만 생성하도록 예외처리 필요함
            skill.SetPlayerReferences(playerTransform, playerMove);
            skill.InitSkillPosition();

            skill.UpgradeSkill(
                damage: _damageMultiplier,
                cd: _cooldownMultiplier,
                projectileNumber: _projectileNumberMultiplier,
                projectileSpeed: _projectileSpeedMultiplier,
                size: _sizeMultiplier,
                duration: _durationMultiplier,
                knockbackForce: _knockbackForceMultiplier
                );
        }
    }


    public void ApplyUpgradeFromCard(UpgradeCardsSO card)
    {
        _damageMultiplier += card.DamageMultiplier;
        _cooldownMultiplier += card.CooldownMultiplier;
        _projectileNumberMultiplier += card.ProjectileNumberMultiplier;
        _projectileSpeedMultiplier += card.ProjectileSpeedMultiplier;
        _sizeMultiplier += card.SizeMultiplier;
        _durationMultiplier += card.DurationMultiplier;
        _knockbackForceMultiplier += card.KnockbackForceMultiplier;
    }


    // TODO: 쿨다운 업그레이드가 되면 이벤트 발생되는 구조, 해당 이벤트에 이 메서드 구독처리 할 것
    //public void UpdateCooldownValue(float newValue)
    //{
    //    Data.Cooldown = newValue;
    //}
}
