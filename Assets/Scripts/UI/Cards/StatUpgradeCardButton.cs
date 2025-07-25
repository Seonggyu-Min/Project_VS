using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatUpgradeCardButton : MonoBehaviour
{
    [SerializeField] private UpgradeCardsSO _upgradeCardsSO;

    // 텍스트
    [SerializeField] private TMP_Text _upperText;
    [SerializeField] private TMP_Text _lowerText;

    private StringBuilder _upSb = new StringBuilder();
    private StringBuilder _lowSb = new StringBuilder();

    private bool _isCardSelected = false;

    private void OnEnable()
    {
        InitText();
        InitCardButton();
    }

    private void InitText()
    {
        _upSb.Clear();
        _upSb.AppendLine(_upgradeCardsSO.SkillName);
        _upperText.text = _upSb.ToString();

        _lowSb.Clear();
        _lowSb.AppendLine("Upgrade Stat");
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
