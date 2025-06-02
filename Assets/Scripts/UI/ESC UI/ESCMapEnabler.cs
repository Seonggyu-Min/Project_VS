using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMapEnabler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bookmarks;
    [SerializeField] private GameObject _backgroundPanel;

    private readonly int Open_Hash = Animator.StringToHash("MapOpen");


    private void OnEnable()
    {
        _animator.Play(Open_Hash);
    }

    // 애니메이션 이벤트용 메서드
    private void EnableESCMap()
    {
        _bookmarks.SetActive(true);
    }

    // 애니메이션 이벤트용 메서드
    private void DisableESCMap()
    {
        _backgroundPanel.SetActive(false);
        gameObject.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
