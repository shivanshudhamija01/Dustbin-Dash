using UnityEngine;
using UnityEngine.UI;

public class GamePausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    private IEventBus eventBus;
    void Awake()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        eventBus = ServiceContainer.Get<IEventBus>();
    }
    private void OnResumeButtonClicked()
    {
        Time.timeScale = 1f;
        eventBus.Publish(new Events.OnGameResumed());
    }
    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1f;
        eventBus.Publish(new Events.OnGameRestarted());
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
