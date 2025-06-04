using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        _bgmSlider.value = TitleGameManager.Instance.AudioManager.BGMVolume.Value;
        _sfxSlider.value = TitleGameManager.Instance.AudioManager.SFXVolume;
    }

    public void OnBGMChanged(Slider slider)
    {
        TitleGameManager.Instance.AudioManager.SetBGMVolume(slider.value);
    }

    public void OnSFXChanged(Slider slider)
    {
        TitleGameManager.Instance.AudioManager.SetSFXVolume(slider.value);
    }

    public void OnClickMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
