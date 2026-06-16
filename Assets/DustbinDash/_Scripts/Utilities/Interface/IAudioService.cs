public interface IAudioService
{
    void PlayBGM(SoundType soundType);
    void PlaySFX(SoundType soundType);
    void PlayUISFX(SoundType soundType);

    void PauseGamePlayAudio();
    void ResumeGamePauseAudio();


    void SetBGMVolume(float value);
}