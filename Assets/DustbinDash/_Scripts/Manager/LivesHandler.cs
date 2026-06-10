using UnityEngine;

public class LivesHandler : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;

    public int Lives { get; private set; }

    private void Awake()
    {
        Lives = startingLives;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<Events.OnWasteMissed>(HandleMiss);
        EventBus.Subscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnWasteMissed>(HandleMiss);
        EventBus.Unsubscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void HandleMiss(Events.OnWasteMissed evt)
    {
        Lives = Mathf.Max(0, Lives - 1);

        EventBus.Publish(
            new Events.OnLivesChanged(Lives));

        if (Lives <= 0)
        {
            EventBus.Publish(
                new Events.OnGameOver());
        }
    }

    private void HandleRestart(Events.OnGameRestarted evt)
    {
        Lives = startingLives;

        EventBus.Publish(
            new Events.OnLivesChanged(Lives));
    }
}