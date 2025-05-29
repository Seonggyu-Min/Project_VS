using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : PooledObject<BaseMonster>, IDamageable
{
    [SerializeField] protected MonstersSO _monstersSO;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] LayerMask _playerLayerMask = 1 << 7;

    [SerializeField] protected float _getRedTimer = 0f;
    [SerializeField] protected float _getRedDuration = 0.5f;
    [SerializeField] protected bool _isReddish = false;
    [SerializeField] protected Color _originColor;

    [SerializeField] protected Animator _animator;

    private readonly int Walk_Hash = Animator.StringToHash("Walk");
    private readonly int Die_Hash = Animator.StringToHash("Die");

    protected int _damage;
    protected int _maxHealth;
    protected int _currentHealth;
    protected float _moveSpeed;
    protected int _dropExp;

    private Transform _target;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public int DropExp => _dropExp;

    protected virtual void Awake()
    {
        _originColor = _spriteRenderer.color;
    }


    protected virtual void FixedUpdate()
    {
        Trace();
        GetRedChecker();
        FilpX();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    protected virtual void Trace()
    {
        Vector2 dir;
        dir.x = _target.position.x - _rb.position.x;
        dir.y = _target.position.y - _rb.position.y;

        _rb.MovePosition(_rb.position + dir.normalized * MoveSpeed * Time.fixedDeltaTime);
    }


    // 왼쪽이 flipX = false로 설정
    protected virtual void FilpX()
    {
        if (_target.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    // TODO: 스폰매니저에서 위치도 지정하면 좋을 듯
    public virtual void InitBaseMonster(Transform target)
    {
        InitChildMonster();
        _target = target;
        ResetMonster();
        gameObject.SetActive(true);
    }

    protected virtual void ResetMonster()
    {
        _isReddish = false;
        _getRedTimer = 0f;
        _spriteRenderer.color = _originColor;
        _animator.Play(Walk_Hash);
    }


    // 공격력, 이동속도, 체력, 경험치 자식에서 초기화
    protected abstract void InitChildMonster();

    public virtual void TakeDamage(int damage)
    {
        //Debug.Log($"{gameObject.name}의 체력이 {Health}만큼 남음");
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _animator.Play(Die_Hash);
        }

        GetRed();
    }


    protected virtual void GetRed()
    {
        _getRedTimer = 0f;
        _isReddish = true;
    }

    protected virtual void GetRedChecker()
    {
        if (_isReddish)
        {
            _spriteRenderer.color = Color.Lerp(_originColor, Color.red, 0.4f);

            _getRedTimer += Time.fixedDeltaTime;

            if (_getRedTimer >= _getRedDuration)
            {
                _spriteRenderer.color = _originColor;
                _isReddish = false;
            }
        }
    }


    // TODO: KillCount 처리, 경험치 드랍 처리
    // 애니메이션 이벤트 호출용 메서드
    protected virtual void Die()
    {
        gameObject.SetActive(false);
        ReturnPool();
    }

    protected virtual void HandleCollision(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayerMask) != 0)
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
