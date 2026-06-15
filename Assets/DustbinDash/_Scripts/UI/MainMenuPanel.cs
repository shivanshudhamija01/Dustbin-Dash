using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button exitButton;
    private IEventBus eventBus;
    private IAudioService audioService;

    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        audioService = ServiceContainer.Get<IAudioService>();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        bgmButton.onClick.AddListener(OnBGMButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void OnPlayButtonClicked()
    {
        audioService.PlaySFX(SoundType.click);
        eventBus.Publish(new Events.OnGameStarted());
    }
    void OnBGMButtonClicked()
    {
        audioService.PlaySFX(SoundType.click);
        // Here i just need to mute and un-mute the audio source and also , just need to 
        // toggle the music icon 
    }
    void OnExitButtonClicked()
    {
        audioService.PlaySFX(SoundType.click);
        Application.Quit();
    }
}
