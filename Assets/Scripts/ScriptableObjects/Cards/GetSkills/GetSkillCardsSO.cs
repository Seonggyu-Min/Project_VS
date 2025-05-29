using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GetSkillCardsSO", menuName = "ScriptableObjects/GetSkillCardsSO", order = 1)]
public class GetSkillCardsSO : ScriptableObject, ICard
{
    public string SkillName;
    public SkillsSO SkillsSO;

    string ICard.SkillName => SkillName;

    public void ApplyGetSkillCard()
    {
        SkillManager.Instance.AddSkill(SkillsSO);
    }
}
