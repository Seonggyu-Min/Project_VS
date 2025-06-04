using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SkillUpgradeCardButton : MonoBehaviour
{
    [SerializeField] private UpgradeCardsSO _upgradeCardsSO;

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
        _upSb.AppendLine(_upgradeCardsSO.SkillName);
        _upperText.text = _upSb.ToString();

        _lowSb.Clear();
        _lowSb.AppendLine("Upgrade Skill");
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

        _isCardSelected = true;

        _upgradeCardsSO?.ApplyUpgradeCard();

        CardDrawManager.Instance.EndEffectOfCardSelection();
    }
}
