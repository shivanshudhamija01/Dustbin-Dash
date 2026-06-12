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
        scoreText.text = "0";
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        openBin.onClick.AddListener(OnOpenBinClicked);
        livesLost = 0;
        EventBus.Subscribe<Events.OnGameRestarted>(RestoreLives);
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
    void OnDestroy()
    {
        EventBus.Unsubscribe<Events.OnGameRestarted>(RestoreLives);
    }
    private void OnPauseButtonClicked()
    {
        Time.timeScale = 0f;
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
    private void RestoreLives(Events.OnGameRestarted evt)
    {
        scoreText.text = "0";
        for (int i = 0; i < lives.Count; i++)
        {
            lives[i].color = new Color(1f, 1f, 1f);
        }
    }
}
