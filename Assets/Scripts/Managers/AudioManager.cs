using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private InGameBGMPlayer _inGameBGMPlayer;

    private float _bgmVolume = 0.5f;
    private float _sfxVolume = 0.5f;

    //public float BGMVolume => _bgmVolume;

    public ObservableProperty<float> BGMVolume { get; private set; } = new(0.5f);

    public float SFXVolume => _sfxVolume;

    private void Start()
    {
        BGMVolume.Subscribe(_inGameBGMPlayer.ChangeInGameBGMVolume);
    }

    private void OnDisable()
    {
        BGMVolume.Unsubscribe(_inGameBGMPlayer.ChangeInGameBGMVolume);
    }

    public void SetBGMVolume(float value)
    {
        BGMVolume.Value = value;
    }

    public void SetSFXVolume(float value)
    {
        _sfxVolume = value;
    }
}
