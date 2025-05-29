using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private SkillsSO _initSkillsData;
    [SerializeField] private SkillPoolManager _skillPoolManager;

    [Header("Skill Slots")]
    [SerializeField] private Dictionary<string, ActiveSkillSlot> _skillDict = new();

    public static SkillManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        // 초기 스킬데이터 추가
        AddSkill(_initSkillsData);
    }

    private void OnDestroy()
    {
        ClearSkills();

        Instance = null;
    }

    public void AddSkill(SkillsSO data)
    {
        if (_skillDict.ContainsKey(data.SkillName))
        {
            Debug.LogWarning($"이미 같은 이름의 스킬이 존재함: {data.SkillName}");
            return;
        }

        ActiveSkillSlot slot = new ActiveSkillSlot(data, this);

        slot.IsReady.Subscribe(isReady =>
        {
            if (isReady)
            {
                slot.TryUseSkill(_playerTransform, _playerMove);
            }
        });

        _skillDict.Add(data.SkillName, slot);

        // 즉시 발동
        slot.TryUseSkill(_playerTransform, _playerMove);
    }

    private void ClearSkills()
    {
        foreach (var slot in _skillDict.Values)
        {
            slot.IsReady.UnsbscribeAll();
        }

        _skillDict.Clear();
    }

    public void ApplyCardUpgrade(UpgradeCardsSO card)
    {
        if (_skillDict.TryGetValue(card.SkillName, out var slot))
        {
            slot.ApplyUpgradeFromCard(card);
        }
        else
        {
            Debug.LogWarning($"스킬 {card.SkillName}을 찾을 수 없습니다. 카드 업그레이드 실패");
        }
    }


    // CardDrawManager 중복 검사용
    public List<string> GetOwnedSkillNames()
    {
        return new List<string>(_skillDict.Keys);
    }
}
