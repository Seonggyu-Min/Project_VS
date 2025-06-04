using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneMananger : MonoBehaviour
{
    [SerializeField] private GameObject _howToPlayPanel;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene(1);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //public void OnClickRestartButton()
    //{
    //    SceneManager.LoadScene(1);
    //}

    public void OnClickHowToPlayButton()
    {
        _howToPlayPanel.SetActive(true);
    }

    //public void OnClickMainMenuButton()
    //{
    //    SceneManager.LoadScene(0);
    //}
}
