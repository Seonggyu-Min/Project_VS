using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public abstract class BaseSkill : PooledObject<BaseSkill>
{
    [SerializeField] protected SkillsSO _skillsSO;

    [SerializeField] protected PlayerMove _playerMove;

    [SerializeField] protected Transform _playerTransform;

    [SerializeField] protected LayerMask _monsterLayermask = 1 << 15;

    [Header("Skill Data")]
    protected string _skillName;
    protected int _damage;
    protected float _cooldown;
    protected int _projectileNumber;
    protected float _projectileSpeed;
    protected float _size;
    protected float _duration;
    protected float _knockbackForce;

    protected float _destoryTimer;
    private float _cooldownTimer;
    private bool _isReady = true;

    // Skill Muliplier

    protected float _damageMultiplier = 1f;
    protected float _cooldownMultiplier = 1f;
    protected float _projectileNumberMultiplier = 1f;
    protected float _projectileSpeedMultiplier = 1f;
    protected float _sizeMultiplier = 1f;
    protected float _durationMultiplier = 1f;
    protected float _knockbackForceMultiplier = 1f;

    public float DamageMultiplier => _damageMultiplier;
    public float CooldownMultiplier => _cooldownMultiplier;
    public float ProjectileNumberMultiplier => _projectileNumberMultiplier;
    public float ProjectileSpeedMultiplier => _projectileSpeedMultiplier;
    public float SizeMultiplier => _sizeMultiplier;
    public float DurationMultiplier => _durationMultiplier;
    public float KnockbackForceMultiplier => _knockbackForceMultiplier;




    public string SkillName => _skillName;
    public int Damage => _damage;
    public float Cooldown => _cooldown;
    public int ProjectileNumber => _projectileNumber;
    public float ProjectileSpeed => _projectileSpeed;
    public float Size => _size;
    public float Duration => _duration;
    public float KnockbackForce => _knockbackForce;
    public float CooldownTimer => _cooldownTimer;
    public bool IsReady => _isReady;

    public SkillsSO SkillsSO => _skillsSO;


    [SerializeField] AudioSource _spawnAudioSource;

    //protected float _SkillSpawnSoundVolume;


    private void Awake()
    {
        InitSkillData();
    }

    protected virtual void OnEnable()
    {
        _destoryTimer = 0f;
        InitSkillSize();
        PlaySpawnAudio();
    }

    protected virtual void Update()
    {
        SetDestroyTime();
        //SetCooldownTime();
    }


    // virtual로 선언하여 상속받은 클래스에서 재정의 가능
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    public void TryUseSkill()
    {
        _isReady = false;
        _cooldownTimer = 0f;
    }

    protected virtual void InitSkillData()
    {
        _skillName = _skillsSO.SkillName;
        _damage = _skillsSO.Damage;
        _cooldown = _skillsSO.Cooldown;
        _projectileNumber = _skillsSO.ProjectileNumber;
        _projectileSpeed = _skillsSO.ProjectileSpeed;
        _size = _skillsSO.Size;
        _duration = _skillsSO.Duration;
        _knockbackForce = _skillsSO.KnockbackForce;
    }

    public void UpgradeSkill(
        float damage = 1f,
        float cd = 1f,
        float projectileNumber = 1f,
        float projectileSpeed = 1f,
        float size = 1f,
        float duration = 1f,
        float knockbackForce = 1f
        )
    {
        if (_damageMultiplier != damage)
        {
            _damageMultiplier = damage;
            _damage = Mathf.RoundToInt(_skillsSO.Damage * damage);
        }
        if (_cooldownMultiplier != cd)
        {
            _cooldownMultiplier = cd;
            _cooldown = Mathf.Max(0.1f, _skillsSO.Cooldown * cd); // 쿨다운은 최소 0.1초
        }
        if (_projectileNumberMultiplier != projectileNumber)
        {
            _projectileNumberMultiplier = projectileNumber;
            _projectileNumber = Mathf.RoundToInt(_skillsSO.ProjectileNumber * projectileNumber);
        }
        if (_projectileSpeedMultiplier != projectileSpeed)
        {
            _projectileSpeedMultiplier = projectileSpeed;
            _projectileSpeed = _skillsSO.ProjectileSpeed * projectileSpeed;
        }
        if (_sizeMultiplier != size)
        {
            _sizeMultiplier = size;
            _size = _skillsSO.Size * size;
        }
        if (_durationMultiplier != duration)
        {
            _durationMultiplier = duration;
            _duration = _skillsSO.Duration * duration;
        }
        if (_knockbackForceMultiplier != knockbackForce)
        {
            _knockbackForceMultiplier = knockbackForce;
            _knockbackForce = _skillsSO.KnockbackForce * knockbackForce;
        }
    }

    public void SetPlayerReferences(Transform playerTransform, PlayerMove playerMove)
    {
        _playerTransform = playerTransform;
        _playerMove = playerMove;
    }

    public abstract void InitSkillPosition();

    protected virtual void InitSkillSize()
    {
        gameObject.transform.localScale = new Vector3(_size, _size, 1f);
    }

    protected virtual void SetDestroyTime()
    {
        _destoryTimer += Time.deltaTime;

        if (_destoryTimer >= _duration)
        {
            ReturnPool();
        }
    }


    // Trigger 되었을 때, 데미지를 입힘
    protected virtual void HandleCollision(Collider2D collision)
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
                damageable.TakeDamage(_damage);
            }
            else
            {
                Debug.LogWarning($"IDamageable이 {collision.gameObject.name}에 없음");
            }
        }
    }

    protected virtual void PlaySpawnAudio()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        _spawnAudioSource.pitch = randomPitch;
        _spawnAudioSource.volume = TitleGameManager.Instance.AudioManager.SFXVolume;
        _spawnAudioSource.Play();
    }
}
