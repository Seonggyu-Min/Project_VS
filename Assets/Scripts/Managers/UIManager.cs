using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _paperMap;
    [SerializeField] private GameObject _backgroundPanel;

    [SerializeField] private Animator _paperMapAnimator;
    [SerializeField] private GameObject _bookmarks;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _miniMapPanel;
    [SerializeField] private GameObject _statsPanel;

    private readonly int Close_Hash = Animator.StringToHash("MapClose");

    private bool _isPaused = false;

    private void OnEnable()
    {
        _isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                _bookmarks.SetActive(false);
                _settingsPanel.SetActive(false);
                _miniMapPanel.SetActive(false);
                _statsPanel.SetActive(false);

                _paperMapAnimator.Play(Close_Hash); // 애니메이션 이벤트로 처리

                _isPaused = false;
            }
            else
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                _paperMap.SetActive(true);
                _backgroundPanel.SetActive(true);

                _isPaused = true;
            }
        }
    }
}
