using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private InGameBGMPlayer _inGameBGMPlayer;
    private TitleBGMPlayer _titleBGMPlayer;
    private EndBGMPlayer _endBGMPlayer;

    private float _bgmVolume = 0.5f;
    private float _sfxVolume = 0.5f;

    //public float BGMVolume => _bgmVolume;

    public ObservableProperty<float> BGMVolume { get; private set; } = new(0.5f);

    public float SFXVolume => _sfxVolume;

    public void SetBGMVolume(float value)
    {
        BGMVolume.Value = value;
    }

    public void SetSFXVolume(float value)
    {
        _sfxVolume = value;
    }

    public void RegisterBGMPlayer(InGameBGMPlayer bgm)
    {
        if (_inGameBGMPlayer != null)
        {
            BGMVolume.Unsubscribe(_inGameBGMPlayer.ChangeInGameBGMVolume);
        }

        _inGameBGMPlayer = bgm;
        BGMVolume.Subscribe(_inGameBGMPlayer.ChangeInGameBGMVolume);

        _inGameBGMPlayer.ChangeInGameBGMVolume(BGMVolume.Value);
    }

    public void RegisterBGMPlayer(TitleBGMPlayer bgm)
    {
        if (_titleBGMPlayer != null)
        {
            BGMVolume.Unsubscribe(_titleBGMPlayer.ChangeTitleBGMVolume);
        }

        _titleBGMPlayer = bgm;
        BGMVolume.Subscribe(_titleBGMPlayer.ChangeTitleBGMVolume);

        _titleBGMPlayer.ChangeTitleBGMVolume(BGMVolume.Value);
    }

    public void RegisterBGMPlayer(EndBGMPlayer bgm)
    {
        if (_endBGMPlayer != null)
        {
            BGMVolume.Unsubscribe(_endBGMPlayer.ChangeEndBGMVolume);
        }
        _endBGMPlayer = bgm;
        BGMVolume.Subscribe(_endBGMPlayer.ChangeEndBGMVolume);

        _endBGMPlayer.ChangeEndBGMVolume(BGMVolume.Value);
    }

    public void UnRegisterBGMPlayer(InGameBGMPlayer bgm)
    {
        if (_inGameBGMPlayer == bgm)
        {
            BGMVolume.Unsubscribe(_inGameBGMPlayer.ChangeInGameBGMVolume);
            _inGameBGMPlayer = null;
        }
    }

    public void UnRegisterBGMPlayer(TitleBGMPlayer bgm)
    {
        if (_titleBGMPlayer == bgm)
        {
            BGMVolume.Unsubscribe(_titleBGMPlayer.ChangeTitleBGMVolume);
            _titleBGMPlayer = null;
        }
    }

    public void UnRegisterBGMPlayer(EndBGMPlayer bgm)
    {
        if (_endBGMPlayer == bgm)
        {
            BGMVolume.Unsubscribe(_endBGMPlayer.ChangeEndBGMVolume);
            _endBGMPlayer = null;
        }
    }
}
