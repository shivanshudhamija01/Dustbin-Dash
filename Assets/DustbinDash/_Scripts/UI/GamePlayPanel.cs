using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button openBin;
    [SerializeField] private List<Image> lives;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int maxLives;
    private int livesLost;
    void Awake()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        openBin.onClick.AddListener(OnOpenBinClicked);
        livesLost = 0;
    }
    void OnEnable()
    {
        EventBus.Subscribe<Events.OnScoreAdded>(UpdateScore);
        EventBus.Subscribe<Events.OnLivesChanged>(UpdateLivesIcon);
    }
    void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnScoreAdded>(UpdateScore);
        EventBus.Unsubscribe<Events.OnLivesChanged>(UpdateLivesIcon);
    }
    private void OnPauseButtonClicked()
    {
        EventBus.Publish(new Events.OnGamePaused());
    }
    private void OnOpenBinClicked()
    {
    }
    private void UpdateScore(Events.OnScoreAdded evt)
    {
        int score = evt.Score;
        scoreText.text = score.ToString();
    }
    private void UpdateLivesIcon(Events.OnLivesChanged evt)
    {
        int remainingLives = evt.Lives;
        livesLost = maxLives - remainingLives - 1;
        if (livesLost >= 0)
        {
            lives[livesLost].color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
