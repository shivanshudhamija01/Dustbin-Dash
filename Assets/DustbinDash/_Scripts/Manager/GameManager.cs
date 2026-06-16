using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IScoreService scoreService;
    private IEventBus eventBus;
    private IInputService inputService;
    private IAudioService audioService;
    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        scoreService = ServiceContainer.Get<IScoreService>();
        inputService = ServiceContainer.Get<IInputService>();
        audioService = ServiceContainer.Get<IAudioService>();
    }
    private void OnEnable()
    {
        eventBus.Subscribe<Events.OnLivesDepleted>(HandleLivesDepleted);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnLivesDepleted>(HandleLivesDepleted);
    }
    private void HandleLivesDepleted(Events.OnLivesDepleted evt)
    {
        scoreService.SaveHighScore();

        int currentScore = scoreService.CurrentScore;
        int highScore = scoreService.HighScore;

        inputService.ResetInput();
        audioService.PlaySFX(SoundType.gameover);
        eventBus.Publish(new Events.OnGameOver(currentScore, highScore));
    }

}