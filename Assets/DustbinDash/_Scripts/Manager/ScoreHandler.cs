using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private IEventBus eventBus;
    private IScoreService scoreService;
    private void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        scoreService = ServiceContainer.Get<IScoreService>();
    }

    private void OnEnable()
    {
        eventBus.Subscribe<Events.OnWasteCaught>(HandleCatch);
        eventBus.Subscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnWasteCaught>(HandleCatch);
        eventBus.Unsubscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void HandleCatch(Events.OnWasteCaught evt)
    {
        int previousLevel = scoreService.CurrentLevel;

        int points = evt.Waste.GetScore() * scoreService.CurrentLevel;

        scoreService.AddScore(points);

        eventBus.Publish(new Events.OnScoreAdded(scoreService.CurrentScore));

        if (previousLevel != scoreService.CurrentLevel)
        {
            eventBus.Publish(new Events.OnLevelChanged(scoreService.CurrentLevel));
        }
    }

    private void HandleRestart(Events.OnGameRestarted evt)
    {
        scoreService.ResetProgress();

        eventBus.Publish(new Events.OnScoreAdded(scoreService.CurrentScore));

        eventBus.Publish(new Events.OnLevelChanged(scoreService.CurrentLevel));
    }
}
