using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoolManager : MonoBehaviour
{
    private Dictionary<BaseSkill, ObjectPool<BaseSkill>> _skillPools = new();

    public static SkillPoolManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void CreatePool(BaseSkill skillPrefab, int count = 10)
    {
        if (_skillPools.ContainsKey(skillPrefab)) return;

        ObjectPool<BaseSkill> pool = new ObjectPool<BaseSkill>(transform, skillPrefab, count);
        _skillPools.Add(skillPrefab, pool);
    }

    public BaseSkill GetSkillInstance(BaseSkill skillPrefab)
    {
        if (!_skillPools.ContainsKey(skillPrefab))
        {
            CreatePool(skillPrefab, 10); // 필요 시 즉시 생성
        }

        return _skillPools[skillPrefab].PopPool();
    }

    public void ReturnSkillInstance(BaseSkill instance)
    {
        foreach (var pair in _skillPools)
        {
            if (pair.Key.name == instance.name.Replace("(Clone)", "").Trim())
            {
                pair.Value.PushPool(instance);
                return;
            }
        }

        Destroy(instance.gameObject); // 못 찾았을 경우 파괴
    }
}
