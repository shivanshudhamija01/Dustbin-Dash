using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private IEventBus eventBus;
    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        eventBus.Subscribe<Events.OnGameOver>(UpdateScore);
    }
    void OnDestroy()
    {
        eventBus.Unsubscribe<Events.OnGameOver>(UpdateScore);
    }
    void OnRestartButtonClicked()
    {
        Time.timeScale = 1f;
        eventBus.Publish(new Events.OnGameRestarted());
    }
    void OnExitButtonClicked()
    {
        Application.Quit();
    }
    void UpdateScore(Events.OnGameOver evt)
    {

        int currentScore = evt.Score;
        int highScore = evt.HighScore;

        Debug.Log("Current Score is : " + currentScore + " " + "High Score is : " + highScore);
        scoreText.text = currentScore.ToString();
        bestScoreText.text = highScore.ToString();
    }
}
