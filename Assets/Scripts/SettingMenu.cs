using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [SerializeField] private Toggle _displayDmgToggle;

    private void OnEnable()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("music", 1f);
        _sfxSlider.value = PlayerPrefs.GetFloat("sfx", 1f);
        _displayDmgToggle.isOn = PlayerPrefs.GetInt("displayDamage", 0) == 1;
    }

    #region Audio Setting
    public void SetMusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(_musicSlider.value);
    }
    public void SetSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(_sfxSlider.value);
    }
    #endregion

    public void OnDisplayDmgBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        PlayerPrefs.SetInt("displayDamage", _displayDmgToggle.isOn ? 1 : 0);
    }
}
