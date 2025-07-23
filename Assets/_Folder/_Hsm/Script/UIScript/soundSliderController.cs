using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class soundSliderController : MonoBehaviour
{
    [Header("Sound Setting")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider sfxVolume;

    private void Start()
    {
        LoadSettingsToUI();

        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        bgmVolume.onValueChanged.AddListener(SetBGMVolume);
        sfxVolume.onValueChanged.AddListener(SetSFXVolume);
    }

    private void LoadSettingsToUI()
    {
        GameSetting settings = DataManager_J.instance.setting;

        masterVolume.value = settings.masterVolume;
        bgmVolume.value = settings.bgmVolume;
        sfxVolume.value = settings.sfxVolume;

        SetMasterVolume(settings.masterVolume);
        SetBGMVolume(settings.bgmVolume);
        SetSFXVolume(settings.sfxVolume);
    }

    public void SetMasterVolume(float value)
    {
        float volume = value > 0.0001f ? Mathf.Log10(value) * 20 : -80f;
        mixer.SetFloat("MASTER", volume);
        DataManager_J.instance.setting.masterVolume = value;
    }

    public void SetBGMVolume(float value)
    {
        float volume = value > 0.0001f ? Mathf.Log10(value) * 20 : -80f;
        mixer.SetFloat("BGM", volume);
        DataManager_J.instance.setting.bgmVolume = value;
    }

    public void SetSFXVolume(float value)
    {
        float volume = value > 0.0001f ? Mathf.Log10(value) * 20 : -80f;
        mixer.SetFloat("SFX", volume);
        DataManager_J.instance.setting.sfxVolume = value;
    }

    public void OnClickConfirmReset()
    {
        DataManager_J.instance.setting = new GameSetting();

        LoadSettingsToUI();
        ApplyAndSaveChanges();
    }

    public void ApplyAndSaveChanges()
    {
        DataManager_J.instance.SaveSettings();
    }
}
