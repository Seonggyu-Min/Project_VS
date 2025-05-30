using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Dictionary<BaseMonster, ObjectPool<BaseMonster>> _pools = new ();

    // n번째 웨이브 분리용 리스트 분리
    [SerializeField] private List<BaseMonster> monsterPrefabs_1;
    [SerializeField] private List<BaseMonster> monsterPrefabs_2;
    [SerializeField] private List<BaseMonster> monsterPrefabs_3;
    [SerializeField] private List<BaseMonster> monsterPrefabs_4;
    [SerializeField] private List<BaseMonster> monsterPrefabs_5;
    [SerializeField] private List<BaseMonster> monsterPrefabs_6;
    [SerializeField] private List<BaseMonster> monsterPrefabs_7;

    [SerializeField] private float _spawnTimer;
    [SerializeField] private float _spawnTimerMax = 2f;

    [SerializeField] private Transform _target;


    public static SpawnManager Instance { get; private set; }


    private void Awake()
    {
        Init();

        // 싱글톤 처럼 사용하기 위함
        Instance = this;
    }



    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnTimerMax)
        {
            _spawnTimer = 0f;

            Spawn(monsterPrefabs_1[0], transform.position, _target);
            Spawn(monsterPrefabs_1[1], transform.position, _target);
        }
    }


    private void OnDestroy()
    {
        Instance = null;
    }



    private void Init()
    {
        foreach (BaseMonster prefab in monsterPrefabs_1)
        {
            _pools[prefab] = new ObjectPool<BaseMonster>(transform, prefab, 20);
        }
    }

    private BaseMonster Spawn(BaseMonster prefab, Vector2 pos, Transform target)
    {
        BaseMonster monster = _pools[prefab].PopPool();
        monster.transform.position = pos;
        monster.InitBaseMonster(target);
        return monster;
    }
}
