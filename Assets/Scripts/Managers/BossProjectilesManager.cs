using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectilesManager : MonoBehaviour
{
    [SerializeField] private BossProjectilesBehaviour _projectilePrefab;
 
    private ObjectPool<BossProjectilesBehaviour> _pools;

    public static BossProjectilesManager Instance { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _pools = new ObjectPool<BossProjectilesBehaviour>(transform, _projectilePrefab, 50);
    }

    public BossProjectilesBehaviour GetProjectileInstance()
    {
        return _pools.PopPool();
    }
}
