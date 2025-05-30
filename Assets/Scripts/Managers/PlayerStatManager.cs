using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour, IDamageable
{
    public static PlayerStatManager Instance { get; private set; }
    [Header("Player Stats")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _healthRegen = 1f;
    [SerializeField] private int _armor = 20;

    public ObservableProperty<float> MoveSpeed { get; private set; } = new(2f);

    [SerializeField] private float _gracePeriod = 0.3f;
    [SerializeField] private float _expMultiplied = 1f;
    [SerializeField] private float _goldMultiplied = 1f;
    [SerializeField] private float _magnetRangeMultiplied = 1f;
    [SerializeField] private float _luckMultiplied = 1f;
    [SerializeField] private int _reviveMultiplied = 0;


    [SerializeField] private int _currentExp = 0;
    public ObservableProperty<int> CurrentLevel { get; private set; } = new(1);
    [SerializeField] private int _currentGold = 0;



    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private OverallStatText _overallStatText;

    private Coroutine _graceTimeCoroutine;
    private Coroutine _graceFlickerCoroutine;
    private bool _canTakeDamage = true;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public float HealthRegen => _healthRegen;
    public int Armor => _armor;
    public float GracePeriod => _gracePeriod;
    public float ExpMultiplier => _expMultiplied;
    public float GoldMultiplier => _goldMultiplied;
    public float MagnetRangeMultiplier => _magnetRangeMultiplied;
    public float LuckMultiplier => _luckMultiplied;
    public int ReviveMultiplier => _reviveMultiplied;

    public int CurrentExp => _currentExp;
    public int CurrentGold => _currentGold;



    private void Awake()
    {
        Instance = this;

        InitStat();
    }

    private void Start()
    {
        CurrentLevel.Subscribe(CardDrawManager.Instance.ShowCard);
        MoveSpeed.Subscribe(_playerMove.SetMoveSpeedByStat);
    }

    private void OnDisable()
    {
        MoveSpeed.Unsubscribe(_playerMove.SetMoveSpeedByStat);
        CurrentLevel.Unsubscribe(CardDrawManager.Instance.ShowCard);

        Instance = null;
    }

    public void ApplyCardUpgrade(UpgradeCardsSO card)
    {
        _maxHealth = Mathf.RoundToInt(_maxHealth * (1 + card.HealthMultiplier));
        _healthRegen *= (1 + card.HealthRegenMultiplier);
        _armor = Mathf.RoundToInt(_armor * (1 + card.ArmorMultiplier));
        MoveSpeed.Value *= (1 + card.MoveSpeedMultiplier);
        _gracePeriod *= (1 + card.GracePeriodMultiplier);
        _expMultiplied *= (1 + card.ExpMultiplier);
        _goldMultiplied *= (1 + card.GoldMultiplier);
        _magnetRangeMultiplied *= (1 + card.MagnetRangeMultiplier);
        _luckMultiplied *= (1 + card.LuckMultiplier);
        _reviveMultiplied += card.ReviveMultiplier;

        _overallStatText.RenewStatText();
    }

    private void InitStat()
    {
        _currentHealth = _maxHealth; // 초기 체력 설정
        MoveSpeed.Value = 2f; // 초기 이동 속도 설정
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            _graceTimeCoroutine = StartCoroutine(GraceTime()); // 무적 시간 코루틴
            _graceFlickerCoroutine = StartCoroutine(GraceFlick()); // 깜빡임 코루틴

            // 비율 감산
            float damageMultiplier = 1f - _armor / (_armor + 100f);
            _currentHealth -= Mathf.RoundToInt(damage * damageMultiplier);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void GetExp(int value)
    {
        _currentExp += value;

        if (_currentExp >= CurrentLevel.Value * 50)
        {
            _currentExp -= CurrentLevel.Value * 50;
            CurrentLevel.Value++;
        }

        _overallStatText.RenewStatText();
    }

    [ContextMenu("Test Get Exp")]
    public void TestGetExp()
    {
        int value = 30;
        _currentExp += value;

        if (_currentExp >= CurrentLevel.Value * 50)
        {
            _currentExp -= CurrentLevel.Value * 50;
            CurrentLevel.Value++;
        }

        _overallStatText.RenewStatText();
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
}
