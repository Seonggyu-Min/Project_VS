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


    private void Awake()
    {
        InitSkillData();
    }

    protected virtual void OnEnable()
    {
        _destoryTimer = 0f;
        InitSkillSize();
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
        float damage = 0,
        float cd = 0,
        float projectileNumber = 0,
        float projectileSpeed = 0,
        float size = 0,
        float duration = 0,
    float knockbackForce = 0
        )
    {
        _damage = Mathf.RoundToInt(Damage * damage);

        if (_cooldown * cd >= 0.1f)
        {
            _cooldown *= cd;
        }
        else
        {
            _cooldown = 0.1f;
            Debug.Log("쿨다운은 0.1미만이 될 수 없음");
        }

        _projectileNumber = Mathf.RoundToInt(_projectileNumber * projectileNumber);

        _projectileSpeed *= projectileSpeed;
        _size *= size;
        _duration *= duration;
        _knockbackForce *= knockbackForce;
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
}
