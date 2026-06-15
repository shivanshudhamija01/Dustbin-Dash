using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IScoreService scoreService;
    private IEventBus eventBus;
    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        scoreService = ServiceContainer.Get<IScoreService>();

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

        eventBus.Publish(new Events.OnGameOver(currentScore, highScore));
    }

}