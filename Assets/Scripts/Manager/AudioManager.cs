using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType { combatTheme1, combatTheme2, combatTheme3, menuTheme };
public enum SFXType { btnClick, btnPlay, btnUpgrade, tank1Shoot, tank2Shoot, tank3Shoot, tank4Shoot };

public class AudioManager : Singleton<AudioManager>
{

    [Header("---------Audio Source------------")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] protected AudioSource _sfxSource;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
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
