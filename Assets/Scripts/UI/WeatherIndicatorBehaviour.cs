using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class WeatherIndicatorBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _playTimeText;


    private readonly int _stage2Hash = Animator.StringToHash("Stage2");
    private readonly int _stage3Hash = Animator.StringToHash("Stage3");
    private readonly int _stage4Hash = Animator.StringToHash("Stage4");
    private readonly int _stage5Hash = Animator.StringToHash("Stage5");
    private readonly int _stage6Hash = Animator.StringToHash("Stage6");
    private readonly int _stage7Hash = Animator.StringToHash("Stage7");

    private List<int> _stageList = new();

    private StringBuilder _sb = new();


    private void Awake()
    {
        _stageList.Add(_stage2Hash);
        _stageList.Add(_stage3Hash);
        _stageList.Add(_stage4Hash);
        _stageList.Add(_stage5Hash);
        _stageList.Add(_stage6Hash);
        _stageList.Add(_stage7Hash);
    }

    private void Update()
    {
        _sb.Clear();

        int hours = (int)(WeaterManager.Instance.PlayTime / 3600);
        int minutes = (int)((WeaterManager.Instance.PlayTime % 3600) / 60);
        int seconds = (int)(WeaterManager.Instance.PlayTime % 60);

        _sb.Append($"{hours:D2} : {minutes:D2} : {seconds:D2}");
        _playTimeText.text = _sb.ToString();
    }

    public void SetWeather(int currentStage)
    {
        _animator.Play(_stageList[currentStage - 2]);
    }
}
