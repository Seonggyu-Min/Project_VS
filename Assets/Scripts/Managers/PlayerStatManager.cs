using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerStatManager : MonoBehaviour, IDamageable
{
    public static PlayerStatManager Instance { get; private set; }


    [Header("Components")]
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private HPbarBehaviour _hpBarBehaviour;
    [SerializeField] private EXPbarBehaviour _expBarBehaviour;
    [SerializeField] private AudioSource _hitSoundSource;

    #region Stats Fields
    [SerializeField] private float _healthRegen = 1f;
    [SerializeField] private int _armor = 20;
    [SerializeField] private float _gracePeriod = 0.3f;
    [SerializeField] private float _expMultiplied = 1f;
    [SerializeField] private float _goldMultiplied = 1f;
    [SerializeField] private float _magnetRangeMultiplied = 1f;
    [SerializeField] private float _luckMultiplied = 1f;
    [SerializeField] private int _reviveMultiplied = 0;

    [SerializeField] private int _currentGold = 0;

    #endregion

    #region Stats Properties
    public ObservableProperty<int> MaxHealth { get; private set; } = new(100);
    public ObservableProperty<int> CurrentHealth { get; private set; } = new();
    public ObservableProperty<float> MoveSpeed { get; private set; } = new(2f);
    public ObservableProperty<int> CurrentExp { get; private set; } = new();
    public ObservableProperty<int> CurrentLevel { get; private set; } = new(1);
    public float HealthRegen => _healthRegen;
    public int Armor => _armor;
    public float GracePeriod => _gracePeriod;
    public float ExpMultiplier => _expMultiplied;
    public float GoldMultiplier => _goldMultiplied;
    public float MagnetRangeMultiplier => _magnetRangeMultiplied;
    public float LuckMultiplier => _luckMultiplied;
    public int ReviveMultiplier => _reviveMultiplied;
    public int RequiredExpForNextLevel => CurrentLevel.Value * 50; // 레벨업에 필요한 경험치
    public int CurrentGold => _currentGold;

    #endregion

    #region Local Variables

    private float _regenTimer = 0f;
    private float _regenInterval = 5f; // 체력 재생 간격
    private float _regenDecimalStorage = 0f; // 체력 재생을 위한 소수점 저장소

    private Coroutine _graceTimeCoroutine;
    private Coroutine _graceFlickerCoroutine;
    private bool _canTakeDamage = true;

    #endregion


    private void Awake()
    {
        Instance = this;

        InitStat();
    }

    private void Start()
    {
        CurrentLevel.Subscribe(CardDrawManager.Instance.ShowCard);
        CurrentLevel.Subscribe(_expBarBehaviour.RenewEXPBar);
        CurrentExp.Subscribe(_expBarBehaviour.RenewEXPBar);
        MoveSpeed.Subscribe(_playerMove.SetMoveSpeedByStat);
        CurrentHealth.Subscribe(_hpBarBehaviour.RenewHPBar);
        MaxHealth.Subscribe(_hpBarBehaviour.RenewHPBar);
    }

    private void Update()
    {
        // 테스트용 경험치 업, 테스트 이후 삭제예정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetExp(50);
            Debug.Log("테스트용 경험치 획득");
        }

        HealthRegenerate();
    }

    private void OnDisable()
    {
        MoveSpeed.Unsubscribe(_playerMove.SetMoveSpeedByStat);
        CurrentLevel.Unsubscribe(_expBarBehaviour.RenewEXPBar);
        CurrentLevel.Unsubscribe(CardDrawManager.Instance.ShowCard);
        CurrentExp.Unsubscribe(_expBarBehaviour.RenewEXPBar);
        CurrentHealth.Unsubscribe(_hpBarBehaviour.RenewHPBar);
        MaxHealth.Unsubscribe(_hpBarBehaviour.RenewHPBar);

        Instance = null;
    }

    public void ApplyCardUpgrade(UpgradeCardsSO card)
    {
        MaxHealth.Value = Mathf.RoundToInt(MaxHealth.Value * (1 + card.HealthMultiplier));
        _healthRegen *= (1 + card.HealthRegenMultiplier);
        _armor = Mathf.RoundToInt(_armor * (1 + card.ArmorMultiplier));
        MoveSpeed.Value *= (1 + card.MoveSpeedMultiplier);
        _gracePeriod *= (1 + card.GracePeriodMultiplier);
        _expMultiplied *= (1 + card.ExpMultiplier);
        _goldMultiplied *= (1 + card.GoldMultiplier);
        _magnetRangeMultiplied *= (1 + card.MagnetRangeMultiplier);
        _luckMultiplied *= (1 + card.LuckMultiplier);
        _reviveMultiplied += card.ReviveMultiplier;
    }

    private void InitStat()
    {
        CurrentHealth.Value = MaxHealth.Value; // 초기 체력 설정
        MoveSpeed.Value = 2f; // 초기 이동 속도 설정
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            _graceTimeCoroutine = StartCoroutine(GraceTime()); // 무적 시간 코루틴
            _graceFlickerCoroutine = StartCoroutine(GraceFlick()); // 깜빡임 코루틴

            PlayHitSound();

            // 비율 감산
            float damageMultiplier = 1f - _armor / (_armor + 100f);
            CurrentHealth.Value -= Mathf.RoundToInt(damage * damageMultiplier);

            if (CurrentHealth.Value <= 0)
            {
                Die();
            }
        }
    }

    private void PlayHitSound()
    {
        float randomPitch = Random.Range(0.8f, 1.2f); // 피격 사운드의 피치 랜덤
        _hitSoundSource.pitch = randomPitch;
        _hitSoundSource.volume = GameManager.Instance.AudioManager.SFXVolume;
        _hitSoundSource.Play();
    }

    public void GetExp(int value)
    {
        CurrentExp.Value += Mathf.RoundToInt(value * _expMultiplied);

        if (CurrentExp.Value >= CurrentLevel.Value * 50)
        {
            CurrentExp.Value -= CurrentLevel.Value * 50;
            CurrentLevel.Value++;
        }
    }

    private void Die()
    {
        // TODO: 게임 매니저 호출
    }

    private IEnumerator GraceTime()
    {
        _canTakeDamage = false;

        float graceTimer = 0f;
        while (graceTimer < _gracePeriod)
        {
            graceTimer += Time.deltaTime;
            yield return null;
        }

        _canTakeDamage = true;
    }

    private IEnumerator GraceFlick()
    {
        float flickTimer = 0f;
        float flickingTime = 0.1f;

        while (flickTimer < _gracePeriod)
        {
            flickTimer += flickingTime;

            Color color = _playerSpriteRenderer.color;
            color.a = (color.a == 1f) ? 0.8f : 1f; // 투명도 깜빡임
            _playerSpriteRenderer.color = color;

            yield return new WaitForSeconds(flickingTime);
        }

        Color c = _playerSpriteRenderer.color;
        c.a = 1f; // 투명도 복원
        _playerSpriteRenderer.color = c;
    }

    private void HealthRegenerate()
    {
        _regenTimer += Time.deltaTime;

        if (_regenTimer >= _regenInterval)
        {
            if (_regenDecimalStorage >= 1f)
            {
                CurrentHealth.Value += Mathf.RoundToInt(_healthRegen + _regenDecimalStorage);
                _regenDecimalStorage = _regenDecimalStorage - Mathf.Floor(_regenDecimalStorage); // 소수점만 다시 저장
            }

            _regenDecimalStorage += _healthRegen - Mathf.Floor(_healthRegen); // 소수점 저장
            CurrentHealth.Value = Mathf.Min(CurrentHealth.Value + Mathf.RoundToInt(_healthRegen), MaxHealth.Value);

            _regenTimer = 0f;
        }
    }
}
