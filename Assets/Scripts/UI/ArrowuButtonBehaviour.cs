using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ArrowuButtonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _desc1Panel;
    [SerializeField] private GameObject _desc2Panel;
    [SerializeField] private GameObject _desc3Panel;
    [SerializeField] private GameObject _desc4Panel;

    //[SerializeField] private VideoPlayer _desc1Video;
    //[SerializeField] private VideoPlayer _desc2Video;
    //[SerializeField] private VideoPlayer _desc3Video;
    //[SerializeField] private VideoPlayer _desc4Video;

    private int _currentPanelIndex = 1;
    private int _minPanelIndex = 1;
    private int _maxPanelIndex = 4;

    private void OnEnable()
    {
        _currentPanelIndex = 1;

        UpdatePanels();
    }

    public void OnClickLeftArrow()
    {
        _currentPanelIndex--;

        if (_currentPanelIndex < _minPanelIndex)
        {
            _currentPanelIndex = _maxPanelIndex;
        }

        UpdatePanels();
    }

    public void OnClickRightArrow()
    {
        _currentPanelIndex++;

        if (_currentPanelIndex > _maxPanelIndex)
        {
            _currentPanelIndex = _minPanelIndex;
        }

        UpdatePanels();
    }

    private void UpdatePanels()
    {
        _desc1Panel.SetActive(_currentPanelIndex == 1);
        _desc2Panel.SetActive(_currentPanelIndex == 2);
        _desc3Panel.SetActive(_currentPanelIndex == 3);
        _desc4Panel.SetActive(_currentPanelIndex == 4);

        //_desc1Video.gameObject.SetActive(_currentPanelIndex == 1);
        //_desc2Video.gameObject.SetActive(_currentPanelIndex == 2);
        //_desc3Video.gameObject.SetActive(_currentPanelIndex == 3);
        //_desc4Video.gameObject.SetActive(_currentPanelIndex == 4);
    }

    public void OnClickCloseButton()
    {
        _desc1Panel.SetActive(false);
        _desc2Panel.SetActive(false);
        _desc3Panel.SetActive(false);
        _desc4Panel.SetActive(false);

        //_desc1Video.gameObject.SetActive(false);
        //_desc2Video.gameObject.SetActive(false);
        //_desc3Video.gameObject.SetActive(false);
        //_desc4Video.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
