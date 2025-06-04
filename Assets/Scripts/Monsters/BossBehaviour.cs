using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 좌우 반전
public class BossBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource; // TODO: 피격음, 공격 재생음, clip 리스트로 관리해서 적절한 음원 재생
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private GameObject _shadow;
    [SerializeField] private BossShadowBehaviour _shadowBehaviour;
    [SerializeField] private GameObject _chargeIndicator;
    [SerializeField] private BossChargeIndicatorBehaviour _chargeIndicatorBehaviour;
    [SerializeField] private LayerMask _playerLayer = 1 << 7;
    [SerializeField] private BossProjectilesManager _bossProjectilesManager;
    [SerializeField] private BossHPbarBehaviour _bossHPbarBehaviour;
    [SerializeField] private GameObject _bossHPbar;
    [SerializeField] private GameObject _EXPbar;

    [SerializeField] private int _maxHealth = 5000;
    public int MaxHealth => _maxHealth;

    public ObservableProperty<int> CurrentHealth { get; private set; } = new();

    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _chargeForce = 10f;
    [SerializeField] private int _damage = 100;

    private Transform _targetTransform;

    private BossPhase _currentPhase;
    private BossState _currentState;
    private int _patternIndex;

    private List<BossState> _phase1Patterns = new List<BossState> { BossState.ShootProjectiles, BossState.Trace };
    private List<BossState> _phase2Patterns = new List<BossState> { BossState.ShootProjectiles, BossState.JumpSmash, BossState.Trace };
    private List<BossState> _phase3Patterns = new List<BossState> { BossState.ShootProjectiles, BossState.JumpSmash, BossState.Charge, BossState.Trace };

    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _castHash = Animator.StringToHash("Cast");
    private readonly int _castHash2 = Animator.StringToHash("Cast2");
    private readonly int _castHash3 = Animator.StringToHash("Cast3");
    //private readonly int _hurtHash = Animator.StringToHash("Hurt");
    private readonly int _dieHash = Animator.StringToHash("Die");

    private Coroutine _patternCoroutine;
    private Coroutine _BossCoroutine;
    private float _getRedTimer = 0f;
    private float _getRedDuration = 0.5f;
    private bool _isReddish = false;
    private Color _originColor;

    private void OnEnable()
    {
        InitBoss();
        InitBossHPBar();
    }
    private void Start()
    {
        _targetTransform = PlayerStatManager.Instance.transform;
        _BossCoroutine = StartCoroutine(PatternRoutine());

        InitBossPos();

        CurrentHealth.Subscribe(_bossHPbarBehaviour.RenewHPBar);
    }

    private void FixedUpdate()
    {
        GetRedChecker();
        FlipX();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            PlayerStatManager.Instance.TakeDamage(_damage);
        }
    }

    private void OnDisable()
    {
        CurrentHealth.Unsubscribe(_bossHPbarBehaviour.RenewHPBar);
    }


    private void InitBoss()
    {
        CurrentHealth.Value = _maxHealth;
        _currentPhase = BossPhase.Phase1;
        _currentState = BossState.Idle;
        _patternIndex = 0;
        _animator.Play(_idleHash);

        _originColor = _spriteRenderer.color;
    }

    private void InitBossPos()
    {
        float camDist = 14.22f + 2f; // 2f는 추가 여유 거리

        float randomAngle = Random.Range(0f, 360f);
        float randomRad = randomAngle * Mathf.Deg2Rad;

        Vector2 spawnPoint = _targetTransform.position;
        spawnPoint.x += Mathf.Cos(randomRad) * camDist;
        spawnPoint.y += Mathf.Sin(randomRad) * camDist;

        transform.position = spawnPoint;
    }

    private void InitBossHPBar()
    {
        _EXPbar.SetActive(false);
        _bossHPbar.SetActive(true);
    }

    private void SetPhase()
    {
        float healthRatio = (float)CurrentHealth.Value / _maxHealth;
        if (healthRatio >= 0.66f)
        {
            _currentPhase = BossPhase.Phase1;
        }
        else if (healthRatio >= 0.33f)
        {
            _currentPhase = BossPhase.Phase2;
        }
        else
        {
            _currentPhase = BossPhase.Phase3;
        }
    }

    private IEnumerator PatternRoutine()
    {
        while (CurrentHealth.Value > 0)
        {
            SetPhase();

            List<BossState> currentPatternList = GetPatternListForPhase();
            _currentState = currentPatternList[_patternIndex % currentPatternList.Count];
            _patternIndex++;

            switch (_currentState)
            {
                case BossState.ShootProjectiles:
                    yield return _patternCoroutine = StartCoroutine(ShootProjectilesRoutine());
                    break;

                case BossState.JumpSmash:
                    yield return _patternCoroutine = StartCoroutine(JumpSmashRoutine());
                    break;

                case BossState.Charge:
                    yield return _patternCoroutine = StartCoroutine(ChargeRoutine());
                    break;

                case BossState.Trace:
                    yield return _patternCoroutine = StartCoroutine(TraceRoutine());
                    break;
            }

            yield return _patternCoroutine = StartCoroutine(IdleRoutine());
        }
    }


    private List<BossState> GetPatternListForPhase()
    {
        switch (_currentPhase)
        {
            case BossPhase.Phase1:
                return _phase1Patterns;
            case BossPhase.Phase2:
                return _phase2Patterns;
            case BossPhase.Phase3:
                return _phase3Patterns;
            default:
                Debug.LogWarning($"현재 보스 페이즈가 없음: {_currentPhase}");
                return _phase1Patterns;
        }
    }

    private IEnumerator ShootProjectilesRoutine()
    {
        _animator.Play(_castHash);

        for (int i = 0; i < 50; i++)
        {
            BossProjectilesBehaviour projectile = _bossProjectilesManager.GetProjectileInstance();
            projectile.transform.position = transform.position;
            projectile.SetTarget(_targetTransform);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator JumpSmashRoutine()
    {
        _animator.Play(_castHash);
        _spriteRenderer.enabled = false;
        _shadowBehaviour.InitShadow();

        yield return new WaitForSeconds(_shadowBehaviour.MaxTime + 2f); // 그림자 고정 후 2초 뒤 소환
        _spriteRenderer.enabled = true;
        transform.position = _shadow.transform.position;
    }

    private IEnumerator ChargeRoutine()
    {
        _animator.Play(_castHash2);

        Vector2 targetPositionAtStart = _targetTransform.position;

        _chargeIndicator.SetActive(true);
        yield return new WaitForSeconds(_chargeIndicatorBehaviour.DisappearTime);

        _rb.AddForce((targetPositionAtStart - (Vector2)transform.position).normalized * _chargeForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f); // 잠시 후 돌진 종료
        _rb.velocity = Vector2.zero;
    }

    private IEnumerator TraceRoutine()
    {
        _animator.Play(_castHash3);

        float traceDuration = 10f;
        float timer = 0f;

        while (timer < traceDuration)
        {
            timer += Time.deltaTime;

            Vector2 dir = (_targetTransform.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + dir * _moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator IdleRoutine()
    {
        _animator.Play(_idleHash);
        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(2f);
    }


    public void TakeDamage(int damage)
    {
        CurrentHealth.Value -= damage;

        if (CurrentHealth.Value <= 0)
        {
            Die();
        }

        PlayHitSound();
        GetRed();
    }

    private void Die()
    {
        _animator.Play(_dieHash);

        if (_BossCoroutine != null)
        {
            StopCoroutine(_BossCoroutine);
        }

        if (_patternCoroutine != null)
        {
            StopCoroutine(_patternCoroutine);
        }

        gameObject.SetActive(false);
        GameManager.Instance.WinOrLoseManager.SetWin();
    }

    private void GetRed()
    {
        _getRedTimer = 0f;
        _isReddish = true;
    }

    private void GetRedChecker()
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

    private void FlipX()
    {
        if (_targetTransform.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void PlayHitSound()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        _audioSource.pitch = randomPitch;
        _audioSource.volume = TitleGameManager.Instance.AudioManager.SFXVolume;
        _audioSource.Play();
    }
}

public enum BossState
{
    Idle,
    ShootProjectiles,
    JumpSmash,
    Charge,
    Trace,
    Die
}

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3
}