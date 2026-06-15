using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gamePausePanel;
    [SerializeField] private GameObject gameLostPanel;
    private IEventBus eventBus;
    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        mainMenuPanel.SetActive(true);
        gameLostPanel.SetActive(false);
        gamePausePanel.SetActive(false);
        gamePlayPanel.SetActive(false);
    }
    void OnEnable()
    {
        eventBus.Subscribe<Events.OnGameStarted>(GameStarted);
        eventBus.Subscribe<Events.OnGamePaused>(GamePaused);
        eventBus.Subscribe<Events.OnGameResumed>(GameResumed);
        eventBus.Subscribe<Events.OnLivesDepleted>(GameLost);
        eventBus.Subscribe<Events.OnGameRestarted>(GameRestart);
    }
    void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnGameStarted>(GameStarted);
        eventBus.Unsubscribe<Events.OnGamePaused>(GamePaused);
        eventBus.Unsubscribe<Events.OnGameResumed>(GameResumed);
        eventBus.Unsubscribe<Events.OnLivesDepleted>(GameLost);
        eventBus.Unsubscribe<Events.OnGameRestarted>(GameRestart);
    }
    void GameStarted(Events.OnGameStarted evt)
    {
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }
    void GamePaused(Events.OnGamePaused evt)
    {
        gamePlayPanel.SetActive(false);
        gamePausePanel.SetActive(true);
    }
    void GameResumed(Events.OnGameResumed evt)
    {
        gamePausePanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }
    void GameLost(Events.OnLivesDepleted evt)
    {
        gameLostPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }
    void GameRestart(Events.OnGameRestarted evt)
    {
        gamePausePanel.SetActive(false);
        gameLostPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }
}
