using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaterManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float _playTime = 0f;
    [SerializeField] private float _changeWeatherTime = 180f;

    [Header("Weather Settings")]
    [SerializeField] private Light2D _sunLight;
    [SerializeField] private Color _dawnColor;
    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _noonColor;
    [SerializeField] private Color _nightColor;
    [SerializeField] private Color _rainingColor;
    [SerializeField] private Color _lightningColor1;
    [SerializeField] private Color _lightningColor2;
    [SerializeField] private GameObject _rainParticle;
    [SerializeField] private GameObject _lightningObj1;
    [SerializeField] private GameObject _lightningObj2;
    [SerializeField] private WeatherType _currentWeatherType = WeatherType.Dawn;

    [Header("Lightning Settings")]
    private bool _isPreparingLightning = false; // 번개 준비 상태
    private float _lightningInterval; // 번개 발생 간격
    private float _lightningTimer; // 번개 발생 타이머
    private int _lightningPrefabIndex;
    [SerializeField] private Transform _playerTransform;

    public static WeaterManager Instance { get; private set; }

    public bool IsStarted { get; set; } = false;
    public float PlayTime => _playTime;

    private void Awake()
    {
        Instance = this;
        SetColor();
    }

    private void Update()
    {
        TimeAdder();
        LightningTimeChecker();
    }

    private void FixedUpdate()
    {
        CheckWeather();
    }

    private void OnDisable()
    {
        Instance = null;
    }

    private void SetColor()
    {
        _dawnColor = new Color32(250, 194, 70, 255);
        _dayColor = new Color32(225, 255, 229, 255);
        _noonColor = new Color32(255, 120, 73, 255);
        _nightColor = new Color32(40, 36, 195, 255);
        _rainingColor = new Color32(47, 58, 255, 255);
        _lightningColor1 = new Color32(33, 33, 255, 255);
        _lightningColor2 = new Color32(33, 33, 255, 255);
    }

    private void TimeAdder()
    {
        if (IsStarted)
        {
            _playTime += Time.deltaTime;
        }
    }


    // 플레이 시간에 따른 날씨 변경
    private WeatherType GetWeatherType()
    {
        if (_playTime < _changeWeatherTime) return WeatherType.Dawn;
        else if (_playTime < _changeWeatherTime * 2) return WeatherType.Day;
        else if (_playTime < _changeWeatherTime * 3) return WeatherType.Noon;
        else if (_playTime < _changeWeatherTime * 4) return WeatherType.Night;
        else if (_playTime < _changeWeatherTime * 5) return WeatherType.Raining;
        else if (_playTime < _changeWeatherTime * 6) return WeatherType.Lightning1;
        else return WeatherType.Lightning2;
    }

    // 날씨 변경 체크
    private void CheckWeather()
    {
        if (_currentWeatherType != GetWeatherType())
        {
            _currentWeatherType = GetWeatherType();
            ApplyWeather(_currentWeatherType);
        }
    }

    // 날씨에 따른 분위기 연출 변경
    private void ApplyWeather(WeatherType weather)
    {
        switch (weather)
        {
            case (WeatherType.Dawn):
                _sunLight.intensity = 1.3f;
                _sunLight.color = _dawnColor;
                break;
            case (WeatherType.Day):
                _sunLight.intensity = 1.3f;
                _sunLight.color = _dayColor;
                break;
            case (WeatherType.Noon):
                _sunLight.intensity = 1.3f;
                _sunLight.color = _noonColor;
                break;
            case (WeatherType.Night):
                _sunLight.intensity = 1.5f;
                _sunLight.color = _nightColor;
                break;
            case (WeatherType.Raining):
                _sunLight.intensity = 1.7f;
                _sunLight.color = _rainingColor;
                _rainParticle.SetActive(true); // 비 오기 시작
                break;
            case (WeatherType.Lightning1):
                _sunLight.intensity = 1.3f;
                _sunLight.color = _lightningColor1;
                break;
            case (WeatherType.Lightning2):
                _sunLight.intensity = 1.3f;
                _sunLight.color = _lightningColor2;
                break;
        }
    }


    private void LightningRandomizer()
    {
        _isPreparingLightning = true;

        _lightningTimer = 0f;
        _lightningInterval = Random.Range(1, 5);
        _lightningPrefabIndex = Random.Range(0, 2);

        _lightningPosX = Random.Range(_playerTransform.position.x - 10f, _playerTransform.position.x + 10f);

        _lightningV2.x = _lightningPosX;
        _lightningV2.y = _playerTransform.position.y + 10f;
    }

    private float _lightningPosX;
    private Vector3 _lightningV2;

    private void LightningTimeChecker()
    {
        // 날씨가 Lightning일 때
        if (_playTime >= _changeWeatherTime * 5)
        {
            if (!_isPreparingLightning)
            {
                LightningRandomizer();
            }
            
            _lightningTimer += Time.deltaTime;

            if (_lightningTimer >= _lightningInterval)
            {
                if (_lightningPrefabIndex == 0)
                {
                    _lightningObj1.transform.position = _lightningV2;
                    _lightningObj1.SetActive(true);
                }
                else
                {
                    _lightningObj2.transform.position = _lightningV2;
                    _lightningObj2.SetActive(true);
                }

                _isPreparingLightning = false;
            }
        }
    }



    [ContextMenu("Test Plus Time for 3 minites")]
    private void TestPlusTime()
    {
        IsStarted = true;
        _playTime += 180f;
    }
}

public enum WeatherType
{
    Dawn,
    Day,
    Noon,
    Night,
    Raining,
    Lightning1,
    Lightning2
}
