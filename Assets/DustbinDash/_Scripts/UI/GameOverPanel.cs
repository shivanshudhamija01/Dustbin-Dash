using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    void Awake()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    void OnRestartButtonClicked()
    {
        EventBus.Publish(new Events.OnGameRestarted());
    }
    void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
