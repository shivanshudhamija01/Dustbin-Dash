using System.Collections.Generic;
using UnityEngine;

public class AudioService : IAudioService
{
    private readonly Dictionary<SoundType, SoundMap> audioMap = new();

    private readonly AudioSource bgmSource;
    private readonly AudioSource sfxSource;
    private readonly AudioSource guiSource;

    private float bgmVolume;
    private float sfxVolume;

    private const string BGM_KEY = "BGM_VOLUME";
    private const string SFX_KEY = "SFX_VOLUME";

    public AudioService(AudioManager audioManager)
    {
        bgmSource = audioManager.BGMSource;
        sfxSource = audioManager.SFXSource;
        guiSource = audioManager.GUISource;

        foreach (var sound in audioManager.Audios)
        {
            audioMap[sound.soundType] = sound;
        }

        bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
        guiSource.volume = sfxVolume;
    }

    public void PlayBGM(SoundType soundType)
    {
        if (!audioMap.TryGetValue(soundType, out var sound))
            return;

        if (bgmSource.clip == sound.audioClip &&
            bgmSource.isPlaying)
            return;

        bgmSource.clip = sound.audioClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(SoundType soundType)
    {
        if (!audioMap.TryGetValue(soundType, out var sound))
            return;

        float finalVolume = sound.volume * sfxVolume;

        sfxSource.PlayOneShot(sound.audioClip, finalVolume);
    }

    public void PlayUISFX(SoundType soundType)
    {
        if (!audioMap.TryGetValue(soundType, out var sound))
            return;

        float finalVolume = sound.volume * sfxVolume;

        guiSource.PlayOneShot(sound.audioClip, finalVolume);
    }

    public void PauseGamePlayAudio()
    {
        AudioListener.pause = true;
    }

    public void ResumeGamePlayAudio()
    {
        AudioListener.pause = false;
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);

        bgmSource.volume = bgmVolume;

        PlayerPrefs.SetFloat(BGM_KEY, bgmVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);

        sfxSource.volume = sfxVolume;
        guiSource.volume = sfxVolume;

        PlayerPrefs.SetFloat(SFX_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}