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

    private StringBuilder _upSb = new StringBuilder();
    private StringBuilder _lowSb = new StringBuilder();

    private void OnEnable()
    {
        _upSb.Clear();
        _upSb.AppendLine(_upgradeCardsSO.SkillName);
        _upperText.text = _upSb.ToString();

        _lowSb.Clear();
        _lowSb.AppendLine("Upgrade Skill");
        _lowerText.text = _lowSb.ToString();
    }

    public void OnClickCard()
    {
        _upgradeCardsSO?.ApplyUpgradeCard();

        CardDrawManager.Instance.EndEffectOfCardSelection();
    }
}
