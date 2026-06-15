using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreHandler scoreHandler;
    private IEventBus eventBus;
    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
    }
    private void OnEnable()
    {
        eventBus.Subscribe<Events.OnLivesDepleted>(HandleLivesDepleted);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnLivesDepleted>(HandleLivesDepleted);
    }

    // private void HandleLivesDepleted(Events.OnLivesDepleted evt)
    // {

    //     scoreHandler.SaveHighScore();

    //     EventBus.Publish(new Events.OnGameOver(scoreHandler.Score, scoreHandler.HighScore));
    // }
    private void HandleLivesDepleted(Events.OnLivesDepleted evt)
    {
        Debug.Log($"Score Before Save = {scoreHandler.Score}");
        Debug.Log($"HighScore Before Save = {scoreHandler.HighScore}");

        scoreHandler.SaveHighScore();

        Debug.Log($"Score After Save = {scoreHandler.Score}");
        Debug.Log($"HighScore After Save = {scoreHandler.HighScore}");

        eventBus.Publish(
            new Events.OnGameOver(
                scoreHandler.Score,
                scoreHandler.HighScore));
    }

}