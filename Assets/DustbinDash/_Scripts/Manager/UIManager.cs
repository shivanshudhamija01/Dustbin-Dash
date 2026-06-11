using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gamePausePanel;
    [SerializeField] private GameObject gameLostPanel;

    void Awake()
    {
        mainMenuPanel.SetActive(true);
        gameLostPanel.SetActive(false);
        gamePausePanel.SetActive(false);
        gamePlayPanel.SetActive(false);
    }
    void OnEnable()
    {
        EventBus.Subscribe<Events.OnGameStarted>(GameStarted);
        EventBus.Subscribe<Events.OnGamePaused>(GamePaused);
        EventBus.Subscribe<Events.OnGameResumed>(GameResumed);
        EventBus.Subscribe<Events.OnGameOver>(GameLost);
        EventBus.Subscribe<Events.OnGameRestarted>(GameRestart);
    }
    void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnGameStarted>(GameStarted);
        EventBus.Unsubscribe<Events.OnGamePaused>(GamePaused);
        EventBus.Unsubscribe<Events.OnGameResumed>(GameResumed);
        EventBus.Unsubscribe<Events.OnGameOver>(GameLost);
        EventBus.Unsubscribe<Events.OnGameRestarted>(GameRestart);
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
    void GameLost(Events.OnGameOver evt)
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
