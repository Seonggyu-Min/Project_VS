using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Dictionary<BaseMonster, ObjectPool<BaseMonster>> _pools = new();

    // n번째 웨이브 분리용 리스트 분리
    [SerializeField] private List<BaseMonster> monsterPrefabs_1;
    [SerializeField] private List<BaseMonster> monsterPrefabs_2;
    [SerializeField] private List<BaseMonster> monsterPrefabs_3;
    [SerializeField] private List<BaseMonster> monsterPrefabs_4;
    [SerializeField] private List<BaseMonster> monsterPrefabs_5;
    [SerializeField] private List<BaseMonster> monsterPrefabs_6;

    [SerializeField] private float _spawnTimer;
    [SerializeField] private float _spawnTimerMax = 2f;
    [SerializeField] private float _spawnTimersubtractor = 0.3f;

    [SerializeField] private Transform _playerTransform;

    [SerializeField] private int _currentStage = 1;

    [SerializeField] private GameObject _boss;

    public static SpawnManager Instance { get; private set; }

    [SerializeField] private HashSet<BaseMonster> _spawnedMonsters = new();


    private void Awake()
    {
        Init();

        Instance = this;
    }

    private void Update()
    {
        HandleSpawn();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // 몬스터 프리팹 풀링
    private void Init()
    {
        foreach (BaseMonster prefab in monsterPrefabs_1)
        {
            _pools[prefab] = new ObjectPool<BaseMonster>(transform, prefab, 20);
        }

        foreach (BaseMonster prefab in monsterPrefabs_2)
        {
            _pools[prefab] = new ObjectPool<BaseMonster>(transform, prefab, 20);
        }

        foreach (BaseMonster prefab in monsterPrefabs_3)
        {
            _pools[prefab] = new ObjectPool<BaseMonster>(transform, prefab, 20);
        }

        foreach (BaseMonster prefab in monsterPrefabs_4)
        {
            _pools[prefab] = new ObjectPool<BaseMonster>(transform, prefab, 20);
        }

        foreach (BaseMonster prefab in monsterPrefabs_5)
        {
            _pools[prefab] = new ObjectPool<BaseMonster>(transform, prefab, 20);
        }

        foreach (BaseMonster prefab in monsterPrefabs_6)
        {
            _pools[prefab] = new ObjectPool<BaseMonster>(transform, prefab, 20);
        }
    }

    private void HandleSpawn()
    {
        _spawnTimer += Time.deltaTime;

        switch (_currentStage)
        {
            case 1:
                if (_spawnTimer >= _spawnTimerMax)
                {
                    foreach (var monster in monsterPrefabs_1)
                    {
                        Spawn(monster, GetRandomSpawnPos(), _playerTransform);
                    }

                    _spawnTimer = 0f;
                }
                break;

            case 2:
                if (_spawnTimer >= _spawnTimerMax)
                {
                    foreach (var monster in monsterPrefabs_2)
                    {
                        Spawn(monster, GetRandomSpawnPos(), _playerTransform);
                    }
                    _spawnTimer = 0f;
                }
                break;

            case 3:
                if (_spawnTimer >= _spawnTimerMax)
                {
                    foreach (var monster in monsterPrefabs_3)
                    {
                        Spawn(monster, GetRandomSpawnPos(), _playerTransform);
                    }
                    _spawnTimer = 0f;
                }
                break;

            case 4:
                if (_spawnTimer >= _spawnTimerMax)
                {
                    foreach (var monster in monsterPrefabs_4)
                    {
                        Spawn(monster, GetRandomSpawnPos(), _playerTransform);
                    }
                    _spawnTimer = 0f;
                }
                break;

            case 5:
                if (_spawnTimer >= _spawnTimerMax)
                {
                    foreach (var monster in monsterPrefabs_5)
                    {
                        Spawn(monster, GetRandomSpawnPos(), _playerTransform);
                    }
                    _spawnTimer = 0f;
                }
                break;

            case 6:
                if (_spawnTimer >= _spawnTimerMax)
                {
                    foreach (var monster in monsterPrefabs_6)
                    {
                        Spawn(monster, GetRandomSpawnPos(), _playerTransform);
                    }
                    _spawnTimer = 0f;
                }
                break;

            case 7:
                _boss.SetActive(true);
                break;

            default:
                Debug.LogWarning($"유효하지 않은 스테이지: {_currentStage}");
                break;

        }
    }

    private BaseMonster Spawn(BaseMonster prefab, Vector2 pos, Transform target)
    {
        BaseMonster monster = _pools[prefab].PopPool();
        monster.transform.position = pos;
        monster.InitBaseMonster(target);
        return monster;
    }

    private Vector2 GetRandomSpawnPos()
    {
        // 현재 카메라 비율 16 : 9
        // 카메라 size = 8
        // 세로 = 16
        // 가로 = 16 * (16 / 9) = 28.44
        // 반지름( 28.44 / 2 = 14.22)의 원을 통해 랜덤 위치 생성
        float camDist = 14.22f + 2f; // 2f는 추가 여유 거리

        float randomAngle = Random.Range(0f, 360f);
        float randomRad = randomAngle * Mathf.Deg2Rad;

        Vector2 spawnPoint = _playerTransform.position;
        spawnPoint.x += Mathf.Cos(randomRad) * camDist;
        spawnPoint.y += Mathf.Sin(randomRad) * camDist;

        return spawnPoint;
    }

    public void StageAdder()
    {
        _currentStage++;
        _spawnTimerMax -= _spawnTimersubtractor;
    }

    public void RegisterMonster(BaseMonster monster)
    {
        _spawnedMonsters.Add(monster);
    }

    public void UnregisterMonster(BaseMonster monster)
    {
        _spawnedMonsters.Remove(monster);
    }

    public void StopAllMonster()
    {
        foreach (var mon in _spawnedMonsters)
        {
            mon.IsTimeStopped = true;
        }
    }

    public void ResumeAllMonster()
    {
        foreach (var mon in _spawnedMonsters)
        {
            mon.IsTimeStopped = false;
        }
    }

    public void ClearAllMonsters()
    {
        HashSet<BaseMonster> copy = new HashSet<BaseMonster>(_spawnedMonsters);
        foreach (var mon in copy)
        {
            mon.TakeDamage(9999);
        }
    }
}
