using UnityEngine;
using UnityEngine.UI;

public class GamePausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    private IEventBus eventBus;
    private IAudioService audioService;
    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        audioService = ServiceContainer.Get<IAudioService>();
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private void OnResumeButtonClicked()
    {
        Time.timeScale = 1f;
        audioService.PlaySFX(SoundType.click);
        eventBus.Publish(new Events.OnGameResumed());
    }
    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1f;
        audioService.PlaySFX(SoundType.click);
        eventBus.Publish(new Events.OnGameRestarted());
    }
    private void OnQuitButtonClicked()
    {
        audioService.PlaySFX(SoundType.click);
        Application.Quit();
    }
}
