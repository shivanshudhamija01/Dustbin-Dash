using UnityEngine;

public class LivesHandler : MonoBehaviour
{
    private ILivesService livesService;
    private IEventBus eventBus;

    private void Awake()
    {
        livesService = ServiceContainer.Get<ILivesService>();
        eventBus = ServiceContainer.Get<IEventBus>();
    }

    private void OnEnable()
    {
        eventBus.Subscribe<Events.OnWasteMissed>(HandleMiss);
        eventBus.Subscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnWasteMissed>(HandleMiss);
        eventBus.Unsubscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void HandleMiss(Events.OnWasteMissed evt)
    {
        livesService.LoseLife();

        eventBus.Publish(new Events.OnLivesChanged(livesService.CurrentLives));

        if (livesService.CurrentLives <= 0)
        {
            eventBus.Publish(new Events.OnLivesDepleted());
            Time.timeScale = 0;
        }
    }

    private void HandleRestart(Events.OnGameRestarted evt)
    {
        livesService.ResetLives();

        eventBus.Publish(new Events.OnLivesChanged(livesService.CurrentLives));
    }
}