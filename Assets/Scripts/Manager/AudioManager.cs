using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum MusicType { combatTheme1, combatTheme2, combatTheme3, menuTheme };
public enum SFXType { btnClick, btnPlay, btnUpgrade, tank1Shoot, tank2Shoot, tank3Shoot, tank4Shoot };

public class AudioManager : Singleton<AudioManager>
{

    [Header("---------Audio Source------------")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] protected AudioSource _sfxSource;

    [Header("---------Audio Mixer------------")]
    [SerializeField] private AudioMixer _mixer;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        ApplySavedVolumes();
    }

    // Không thể gọi trong Awake()
    private void ApplySavedVolumes()
    {
        if (_mixer == null) return;

        float music = PlayerPrefs.GetFloat("music", 1f);
        float sfx = PlayerPrefs.GetFloat("sfx", 1f);
        _mixer.SetFloat("music", Mathf.Log10(music) * 20);
        _mixer.SetFloat("sfx", Mathf.Log10(sfx) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        if (_mixer == null) return;
        _mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("music", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (_mixer == null) return;
        _mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfx", volume);
    }
    public void PlaySFX(SFXType type)
    {
        var clip = Resources.Load<AudioClip>($"Sounds/{type}");
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(MusicType type)
    {
        var clip = Resources.Load<AudioClip>($"Musics/{type}");
        _musicSource.clip = clip;
        _musicSource.Play();
    }
}
