using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class KillCountIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _killCountText;
    private StringBuilder _sb = new();

    private void OnEnable()
    {
        RenewText();
    }

    private void RenewText()
    {
        _sb.Clear();
        _sb.AppendLine($"현재 처치 수: {GameManager.Instance.InGameCountManager.KillCount}");

        _killCountText.text = _sb.ToString();
    }
}
