using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        TitleGameManager.Instance.AudioManager.RegisterBGMPlayer(this);
    }

    private void OnDisable()
    {
        TitleGameManager.Instance.AudioManager.UnRegisterBGMPlayer(this);
    }

    public void ChangeEndBGMVolume(float volume)
    {
        _audioSource.volume = volume;
    }
}
