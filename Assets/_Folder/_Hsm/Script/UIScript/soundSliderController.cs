using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundSliderController : MonoBehaviour
{
    public enum VolumeType { Master, BGM, SFX }
    public VolumeType volumeType;

    public Slider slider;

    private void Start()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                slider.value = SoundManager.Instance.masterVolume;
                break;
            case VolumeType.BGM:
                slider.value = SoundManager.Instance.bgmVolume;
                break;
            case VolumeType.SFX:
                slider.value = SoundManager.Instance.sfxVolume;
                slider.value = SoundManager.Instance.uiVolume;
                break;
        }

        slider.onValueChanged.AddListener(OnSliderChanged);
    }
    private void OnSliderChanged(float value)
    {
        switch(volumeType)
        {
            case VolumeType.Master:
                SoundManager.Instance.SetMasterVolume(value);
                break;
            case VolumeType.BGM:
                SoundManager.Instance.SetBGMVolume(value);
                break;
            case VolumeType.SFX:
                SoundManager.Instance.SetSFXVolume(value);
                SoundManager.Instance.uiVolume = value;
                SoundManager.Instance.UpdateVolume();
                break;
        }
    }
}
