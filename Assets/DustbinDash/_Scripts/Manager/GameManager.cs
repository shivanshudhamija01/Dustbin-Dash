using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IScoreService scoreService;
    private IEventBus eventBus;
    private IInputService inputService;
    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        scoreService = ServiceContainer.Get<IScoreService>();
        inputService = ServiceContainer.Get<IInputService>();
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
        eventBus.Publish(new Events.OnGameOver(currentScore, highScore));
    }

}