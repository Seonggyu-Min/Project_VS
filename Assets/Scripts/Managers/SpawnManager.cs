using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Dictionary<BaseMonster, ObjectPool<BaseMonster>> _pools = new ();

    [SerializeField] private List<BaseMonster> monsterPrefabs;

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

            Spawn(monsterPrefabs[0], transform.position, _target);
        }
    }


    private void OnDestroy()
    {
        Instance = null;
    }



    private void Init()
    {
        foreach (BaseMonster prefab in monsterPrefabs)
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
