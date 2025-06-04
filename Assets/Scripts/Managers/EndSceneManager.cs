using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject _clearText;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private TMP_Text _playedTimeText;
    [SerializeField] private TMP_Text _killCountText;

    private StringBuilder _playedTimeSB = new();
    private StringBuilder _killCountSB = new();
    private float _playedTime;


    private void OnEnable()
    {
        IndicateWinOrLose();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void IndicateWinOrLose()
    {
        _playedTime = GameManager.Instance.WinOrLoseManager.PlayedTime;

        int hours = (int)(_playedTime / 3600);
        int minutes = (int)((_playedTime % 3600) / 60);
        int seconds = (int)(_playedTime % 60);

        _killCountSB.Clear();
        _killCountSB.Append($"처치 수: {GameManager.Instance.InGameCountManager.KillCount}");
        _killCountText.text = _killCountSB.ToString();


        if (GameManager.Instance.WinOrLoseManager.IsWin)
        {
            _clearText.SetActive(true);
            _gameOverText.SetActive(false);

            _playedTimeSB.Clear();
            _playedTimeSB.Append($"클리어 시간: {hours:D2} : {minutes:D2} : {seconds:D2}");
            _playedTimeText.text = _playedTimeSB.ToString();
        }
        else
        {
            _clearText.SetActive(false);
            _gameOverText.SetActive(true);

            _playedTimeSB.Clear();
            _playedTimeSB.Append($"플레이 시간: {hours:D2} : {minutes:D2} : {seconds:D2}");
            _playedTimeText.text = _playedTimeSB.ToString();
        }
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(1);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
