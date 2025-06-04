using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : PooledObject<BaseMonster>, IDamageable
{
    [SerializeField] private MonsterFeetPusher _feetPusher;

    [SerializeField] protected MonstersSO _monstersSO;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] LayerMask _playerLayerMask = 1 << 7;

    [SerializeField] protected float _getRedTimer = 0f;
    [SerializeField] protected float _getRedDuration = 0.5f;
    [SerializeField] protected bool _isReddish = false;
    [SerializeField] protected Color _originColor;

    [SerializeField] protected Animator _animator;

    [SerializeField] private PlayerStatManager _playerStatManager;

    private readonly int Walk_Hash = Animator.StringToHash("Walk");
    private readonly int Die_Hash = Animator.StringToHash("Die");

    protected int _damage;
    protected int _maxHealth;
    protected int _currentHealth;
    protected float _moveSpeed;
    protected int _dropExp;

    private Transform _target;

    // 시간 정지용 프로퍼티
    protected bool _isTimeStopped = false;
    public bool IsTimeStopped { get => _isTimeStopped; set => _isTimeStopped = value; }

    public ObservableProperty<bool> IsDead { get; private set; } = new(false);


    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public int DropExp => _dropExp;

    [SerializeField] protected AudioSource _monsterHitSource;
    protected float _monsterHitVolume;

    protected virtual void Awake()
    {
        _originColor = _spriteRenderer.color;
    }

    protected virtual void OnEnable()
    {
        IsDead.Subscribe(_feetPusher.SetDead);
    }
    protected virtual void OnDisable()
    {
        IsDead.Unsubscribe(_feetPusher.SetDead);
    }

    protected virtual void FixedUpdate()
    {
        Trace();
        StopTimeChecker();
        GetRedChecker();
        FilpX();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    protected virtual void Trace()
    {
        if (_isTimeStopped)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        if (!IsDead.Value)
        {
            Vector2 dir;
            dir.x = _target.position.x - _rb.position.x;
            dir.y = _target.position.y - _rb.position.y;

            _rb.MovePosition(_rb.position + dir.normalized * MoveSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void StopTimeChecker()
    {
        if (_isTimeStopped)
        {
            _animator.speed = 0f; // 애니메이션 정지
        }

        else
        {
            _animator.speed = 1f; // 애니메이션 재개
        }
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
        _target = target;
        IsDead.Value = false;
        InitChildMonster();
        ResetMonster();
        gameObject.SetActive(true);
        SpawnManager.Instance.RegisterMonster(this);
    }

    protected virtual void ResetMonster()
    {
        _isReddish = false;
        _isTimeStopped = false;
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

        PlayHitAudio();

        if (_currentHealth <= 0)
        {
            IsDead.Value = true;
            _animator.Play(Die_Hash);

            // 애니메이션 정지상황이라면 즉시 죽음 처리
            if (_isTimeStopped)
            {
                Die();
            }
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
        SpawnManager.Instance.UnregisterMonster(this);
        gameObject.SetActive(false);
        ReturnPool();
        ExpGemBehaviour expGem = PickUpsManager.Instance.GetExpGemInstance();
        expGem.transform.position = transform.position;

        // 아이템 드랍 확률
        float randomMagnetSummonNumber = Random.Range(0f, 1f);

        if (randomMagnetSummonNumber >= 0.99f)
        {
            MagnetPowerUpBehaviour mag = PickUpsManager.Instance.GetMagnetPowerUpInstance();
            Vector2 magSpawnPos = transform.position;
            magSpawnPos.y += 0.3f; // 약간 위로 띄워서 생성
            mag.transform.position = magSpawnPos;
        }

        float randomStopTimeSummonNumber = Random.Range(0f, 1f);

        if (randomStopTimeSummonNumber >= 0.99f)
        {
            StopTimePowerUpBehaviour stopTime = PickUpsManager.Instance.GetStopTimePowerUpInstance();
            Vector2 stopSpawnPos = transform.position;
            stopSpawnPos.y += 0.3f;
            stopTime.transform.position = stopSpawnPos;
        }

        float destroyAllSummonNumber = Random.Range(0f, 1f);

        if (destroyAllSummonNumber >= 0.99f)
        {
            DestroyAllPowerUpBehaviour destroyAll = PickUpsManager.Instance.GetDestroyAllPowerUpInstance();
            Vector2 destroySpawnPos = transform.position;
            destroySpawnPos.y += 0.3f;
            destroyAll.transform.position = destroySpawnPos;
        }

        float randomChestSummonNumber = Random.Range(0f, 1f);

        if (randomChestSummonNumber >= 0.95f)
        {
            ChestPickUpsBehaviour chest = PickUpsManager.Instance.GetChestPowerUpInstance();
            Vector2 chestSpawnPos = transform.position;
            chestSpawnPos.y += 0.3f;
            chest.transform.position = chestSpawnPos;
        }


        GameManager.Instance.InGameCountManager.AddKillCount();
    }

    protected virtual void HandleCollision(Collider2D collision)
    {
        if (_isTimeStopped)
        {
            return;
        }

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


    protected virtual void PlayHitAudio()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        _monsterHitSource.pitch = randomPitch;
        _monsterHitSource.volume = TitleGameManager.Instance.AudioManager.SFXVolume;
        _monsterHitSource.Play();
    }
}
