using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoolManager : MonoBehaviour
{
    public Dictionary<BaseSkill, ObjectPool<BaseSkill>> SkillPools = new();

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
        if (SkillPools.ContainsKey(skillPrefab)) return;

        ObjectPool<BaseSkill> pool = new ObjectPool<BaseSkill>(transform, skillPrefab, count);
        SkillPools.Add(skillPrefab, pool);
    }

    public BaseSkill GetSkillInstance(BaseSkill skillPrefab)
    {
        if (!SkillPools.ContainsKey(skillPrefab))
        {
            CreatePool(skillPrefab, 10); // 필요 시 즉시 생성
        }

        return SkillPools[skillPrefab].PopPool();
    }

    public void ReturnSkillInstance(BaseSkill instance)
    {
        foreach (var pair in SkillPools)
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
