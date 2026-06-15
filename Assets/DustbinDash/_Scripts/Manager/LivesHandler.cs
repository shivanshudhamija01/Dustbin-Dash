using UnityEngine;

public class LivesHandler : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;

    public int Lives { get; private set; }
    private IEventBus eventBus;
    private void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        Lives = startingLives;
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

    // private void HandleMiss(Events.OnWasteMissed evt)
    // {
    //     Lives = Mathf.Max(0, Lives - 1);

    //     // here it will listened in the gameplay panel , to update the lives count
    //     EventBus.Publish(new Events.OnLivesChanged(Lives));

    //     if (Lives <= 0)
    //     {
    //         // Instead of firing the on game over event here, i will fire it in game manager, 
    //         // and here i will fire an lives depleted event which will be listened by the game manager, and that will fire a game over event with scores
    //         Debug.Log("Fire an event here that life is deplected");
    //         EventBus.Publish(new Events.OnLivesDepleted());
    //         // EventBus.Publish(new Events.OnGameOver());
    //     }
    // }
    private void HandleMiss(Events.OnWasteMissed evt)
    {
        Lives = Mathf.Max(0, Lives - 1);
        eventBus.Publish(new Events.OnLivesChanged(Lives));
        if (Lives <= 0)
        {
            eventBus.Publish(new Events.OnLivesDepleted());
            Time.timeScale = 0;
        }
    }

    private void HandleRestart(Events.OnGameRestarted evt)
    {
        Lives = startingLives;

        eventBus.Publish(new Events.OnLivesChanged(Lives));
    }
}