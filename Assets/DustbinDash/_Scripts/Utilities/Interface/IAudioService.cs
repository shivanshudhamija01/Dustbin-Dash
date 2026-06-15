public interface IAudioService
{
    void PlayBGM(SoundType soundType);
    void PlaySFX(SoundType soundType);
    void PlayUISFX(SoundType soundType);

    void PauseGamePlayAudio();
    void ResumeGamePlayAudio();

    void SetBGMVolume(float value);
    void SetSFXVolume(float value);

    float GetBGMVolume();
    float GetSFXVolume();
}