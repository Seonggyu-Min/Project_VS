using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDropManager : MonoBehaviour
{
    private ObjectPool<ExpGemBehaviour> _expGemPool;
    [SerializeField] private ExpGemBehaviour _expGemPrefab;


    public static ExpDropManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        _expGemPool = new ObjectPool<ExpGemBehaviour>(transform, _expGemPrefab, 100);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public ExpGemBehaviour GetExpGemInstance()
    {
        return _expGemPool.PopPool();
    }
}
