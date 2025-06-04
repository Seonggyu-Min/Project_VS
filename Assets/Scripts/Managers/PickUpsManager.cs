using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsManager : MonoBehaviour
{
    [SerializeField] private ExpGemBehaviour _expGemPrefab;
    [SerializeField] private StopTimePowerUpBehaviour _stopTimePowerUpPrefab;
    [SerializeField] private MagnetPowerUpBehaviour _magnetPowerUpPrefab;
    [SerializeField] private DestroyAllPowerUpBehaviour _destroyAllPowerUpPrefab;
    [SerializeField] private ChestPickUpsBehaviour _chestPowerUpPrefab;


    private List<ExpGemBehaviour> _expGemList = new();

    private ObjectPool<ExpGemBehaviour> _expGemPool;
    private ObjectPool<StopTimePowerUpBehaviour> _stopTimePowerUpPool;
    private ObjectPool<MagnetPowerUpBehaviour> _magnetPowerUpPool;
    private ObjectPool<DestroyAllPowerUpBehaviour> _destroyAllPowerUpPool;
    private ObjectPool<ChestPickUpsBehaviour> _chestPowerUpPool;


    public static PickUpsManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        _expGemPool = new ObjectPool<ExpGemBehaviour>(transform, _expGemPrefab, 100);
        _stopTimePowerUpPool = new ObjectPool<StopTimePowerUpBehaviour>(transform, _stopTimePowerUpPrefab, 10);
        _magnetPowerUpPool = new ObjectPool<MagnetPowerUpBehaviour>(transform, _magnetPowerUpPrefab, 10);
        _destroyAllPowerUpPool = new ObjectPool<DestroyAllPowerUpBehaviour>(transform, _destroyAllPowerUpPrefab, 10);
        _chestPowerUpPool = new ObjectPool<ChestPickUpsBehaviour>(transform, _chestPowerUpPrefab, 10);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public ExpGemBehaviour GetExpGemInstance()
    {
        ExpGemBehaviour gem = _expGemPool.PopPool();
        RegisterEXPGem(gem);
        return gem;
    }

    public StopTimePowerUpBehaviour GetStopTimePowerUpInstance()
    {
        return _stopTimePowerUpPool.PopPool();
    }

    public MagnetPowerUpBehaviour GetMagnetPowerUpInstance()
    {
        return _magnetPowerUpPool.PopPool();
    }

    public DestroyAllPowerUpBehaviour GetDestroyAllPowerUpInstance()
    {
        return _destroyAllPowerUpPool.PopPool();
    }

    public ChestPickUpsBehaviour GetChestPowerUpInstance()
    {
        return _chestPowerUpPool.PopPool();
    }


    #region 경험치 보석 관련 메서드
    private void RegisterEXPGem(ExpGemBehaviour gem)
    {
        _expGemList.Add(gem);
    }

    public void UnregisterEXPGem(ExpGemBehaviour gem)
    {
        _expGemList.Remove(gem);
    }

    public void MagnetAllEXPGems()
    {
        foreach (var gem in _expGemList)
        {
            gem.IsAttractedToPlayer = true;
        }
    }
    #endregion
}
