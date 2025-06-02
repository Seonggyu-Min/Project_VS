using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameBGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _inGameBGMList;

    private void OnEnable()
    {
        PlayBGM();
    }

    private void PlayBGM()
    {
        _audioSource.clip = _inGameBGMList[0];
        _audioSource.Play();
    }

    public void ChangeInGameBGMVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public void ChangeBGMIndex(int index)
    {
        if (index < 0 || index >= _inGameBGMList.Length)
        {
            Debug.LogWarning("Invalid BGM index: " + index);
            return;
        }

        _audioSource.clip = _inGameBGMList[index];
        _audioSource.Play();
        Debug.Log("BGM changed to index: " + index);
    }
}
