using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("music"))
            GetMusicVolume();
        else
            SetMusicVolume();

        if (PlayerPrefs.HasKey("sfx"))
            GetSFXVolume();
        else
            SetSFXVolume();
    }
    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("music", volume);
    }
    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        _mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfx", volume);
    }
    private void GetMusicVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("music");
    }
    private void GetSFXVolume()
    {
        _sfxSlider.value = PlayerPrefs.GetFloat("sfx");
    }
}
