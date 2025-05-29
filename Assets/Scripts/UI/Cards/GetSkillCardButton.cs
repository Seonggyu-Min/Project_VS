using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GetSkillCardButton : MonoBehaviour
{
    [SerializeField] private GetSkillCardsSO _getSkillcardSO;
    [SerializeField] private GameObject _relatedCardButton;
    // 텍스트
    [SerializeField] private TMP_Text _upperText;
    [SerializeField] private TMP_Text _lowerText;

    private StringBuilder _upSb = new StringBuilder();
    private StringBuilder _lowSb = new StringBuilder();

    private void OnEnable()
    {
        _upSb.Clear();
        _upSb.AppendLine(_getSkillcardSO.SkillName);
        _upperText.text = _upSb.ToString();

        _lowSb.Clear();
        _lowSb.AppendLine("Get New Skill");
        _lowerText.text = _lowSb.ToString();
    }

    public void OnClickCard()
    {
        _getSkillcardSO?.ApplyGetSkillCard();

        // 자기 자신 삭제 및 업그레이드 카드 등록
        CardDrawManager.Instance.RegisterAndRemoveCard(_relatedCardButton, gameObject);

        CardDrawManager.Instance.EndEffectOfCardSelection();

        //Destroy(gameObject);
    }
}
