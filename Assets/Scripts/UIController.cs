using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    void Update() // 슬라이더 값이 기본 설정값으로 돌아가는 현상을 막기 위해 볼륨값 상시 반영
    {
        _musicSlider.value = SoundManager.Instance.musicSource.volume;
        _sfxSlider.value = SoundManager.Instance.sfxSource.volume;
    }

    public void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        SoundManager.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        SoundManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        SoundManager.Instance.SFXVolume(_sfxSlider.value);
    }
}
