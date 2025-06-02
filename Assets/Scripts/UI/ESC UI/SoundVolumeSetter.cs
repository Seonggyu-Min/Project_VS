using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeSetter : MonoBehaviour
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private void OnEnable()
    {
        SetSliderValue();
    }

    private void SetSliderValue()
    {
        _bgmSlider.value = GameManager.Instance.AudioManager.BGMVolume.Value;
        _sfxSlider.value = GameManager.Instance.AudioManager.SFXVolume;
    }

    public void OnBGMChanged(Slider slider)
    {
        GameManager.Instance.AudioManager.SetBGMVolume(slider.value);
    }

    public void OnSFXChanged(Slider slider)
    {
        GameManager.Instance.AudioManager.SetSFXVolume(slider.value);
    }
}
