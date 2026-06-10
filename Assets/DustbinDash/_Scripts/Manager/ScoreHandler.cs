using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [Header("Level Scaling")]
    [SerializeField] private int pointsPerLevel = 100;
    [SerializeField] private int maxLevel = 10;

    public int Score { get; private set; }
    public int Level { get; private set; } = 1;
    public int HighScore { get; private set; }

    private int nextLevelThreshold;

    private void Awake()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);

        ResetProgress();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<Events.OnWasteCaught>(HandleCatch);
        EventBus.Subscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnWasteCaught>(HandleCatch);
        EventBus.Unsubscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void HandleCatch(Events.OnWasteCaught evt)
    {
        int points =
            evt.Waste.GetScore() * Level;

        Score += points;

        EventBus.Publish(
            new Events.OnScoreAdded(points));

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (Level >= maxLevel)
            return;

        if (Score < nextLevelThreshold)
            return;

        Level++;

        nextLevelThreshold += pointsPerLevel;

        EventBus.Publish(
            new Events.OnLevelChanged(Level));
    }

    private void HandleRestart(Events.OnGameRestarted evt)
    {
        ResetProgress();
    }

    private void ResetProgress()
    {
        Score = 0;
        Level = 1;
        nextLevelThreshold = pointsPerLevel;

        EventBus.Publish(
            new Events.OnLevelChanged(Level));
    }

    public void SaveHighScore()
    {
        if (Score <= HighScore)
            return;

        HighScore = Score;

        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.Save();
    }
}