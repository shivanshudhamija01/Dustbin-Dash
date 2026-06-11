using UnityEngine;
using UnityEngine.UI;

public class GamePausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private void OnResumeButtonClicked()
    {
        EventBus.Publish(new Events.OnGameResumed());
    }
    private void OnRestartButtonClicked()
    {
        EventBus.Publish(new Events.OnGameRestarted());
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
