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

    private bool _isCardSelected = false;

    private StringBuilder _upSb = new StringBuilder();
    private StringBuilder _lowSb = new StringBuilder();

    private void OnEnable()
    {
        InitSkillText();
        InitCardButton();
    }

    private void InitSkillText()
    {
        _upSb.Clear();
        _upSb.AppendLine(_getSkillcardSO.SkillName);
        _upperText.text = _upSb.ToString();

        _lowSb.Clear();
        _lowSb.AppendLine("Get New Skill");
        _lowerText.text = _lowSb.ToString();
    }

    private void InitCardButton()
    {
        _isCardSelected = false;
    }

    public void OnClickCard()
    {
        if (_isCardSelected)
            return;

        _getSkillcardSO?.ApplyGetSkillCard();
        _isCardSelected = true;

        // 자기 자신 삭제 및 업그레이드 카드 등록
        CardDrawManager.Instance.RegisterAndRemoveCard(_relatedCardButton, gameObject);

        CardDrawManager.Instance.EndEffectOfCardSelection();

        //Destroy(gameObject);
    }
}
