using System.Collections.Generic;
using UnityEngine;

public class AudioService : IAudioService
{
    private readonly Dictionary<SoundType, SoundData> audioMap = new();

    private readonly AudioSource bgmSource;
    private readonly AudioSource sfxSource;
    private readonly AudioSource guiSource;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    public AudioService(AudioManager audioManager)
    {
        bgmSource = audioManager.BGMSource;
        sfxSource = audioManager.SFXSource;
        guiSource = audioManager.GUISource;

        foreach (var sound in audioManager.Audios)
        {
            audioMap[sound.soundType] = sound;
        }

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
        sfxSource.Pause();
    }
    public void ResumeGamePauseAudio()
    {
        sfxSource.UnPause();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);

        bgmSource.volume = bgmVolume;
    }
}