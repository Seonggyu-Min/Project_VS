using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookmarksButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _miniMapButton;
    [SerializeField] private Button _statsButton;

    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _miniMapPanel;
    [SerializeField] private GameObject _statsPanel;

    [SerializeField] private RectTransform _settingButtonRect_false;
    [SerializeField] private RectTransform _settingButtonRect_true;
    [SerializeField] private RectTransform _miniMapButtonRect_false;
    [SerializeField] private RectTransform _miniMapButtonRect_true;
    [SerializeField] private RectTransform _statsButtonRect_false;
    [SerializeField] private RectTransform _statsButtonRect_true;

    private bool _isSettingsOpen = true;
    private bool _isMiniMapOpen = false;
    private bool _isStatsOpen = false;

    private void OnEnable()
    {
        InitBookmarks();
    }

    private void InitBookmarks()
    {
        _isSettingsOpen = true;
        _isMiniMapOpen = false;
        _isStatsOpen = false;

        _settingsPanel.SetActive(true);
        _miniMapPanel.SetActive(false);
        _statsPanel.SetActive(false);

        ButtonPosSetter();
    }

    public void OnClickSettingBookmarkButton()
    {
        _isSettingsOpen = true;
        _isMiniMapOpen = false;
        _isStatsOpen = false;

        _settingsPanel.SetActive(true);
        _miniMapPanel.SetActive(false);
        _statsPanel.SetActive(false);

        ButtonPosSetter();
    }
    
    public void OnClickMinimapBookmarkButton()
    {
        _isSettingsOpen = false;
        _isMiniMapOpen = true;
        _isStatsOpen = false;

        _settingsPanel.SetActive(false);
        _miniMapPanel.SetActive(true);
        _statsPanel.SetActive(false);

        ButtonPosSetter();
    }

    public void OnClickStatsBookmarkButton()
    {
        _isSettingsOpen = false;
        _isMiniMapOpen = false;
        _isStatsOpen = true;

        _settingsPanel.SetActive(false);
        _miniMapPanel.SetActive(false);
        _statsPanel.SetActive(true);

        ButtonPosSetter();
    }

    private void ButtonPosSetter()
    {
        if (_isSettingsOpen)    _settingButton.transform.position = _settingButtonRect_true.position;
        else                    _settingButton.transform.position = _settingButtonRect_false.position;

        if (_isMiniMapOpen) _miniMapButton.transform.position = _miniMapButtonRect_true.position;
        else                _miniMapButton.transform.position = _miniMapButtonRect_false.position;

        if (_isStatsOpen) _statsButton.transform.position = _statsButtonRect_true.position;
        else              _statsButton.transform.position = _statsButtonRect_false.position;
    }
}
