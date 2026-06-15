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
    private IEventBus eventBus;
    private void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        ResetProgress();
    }

    private void OnEnable()
    {
        eventBus.Subscribe<Events.OnWasteCaught>(HandleCatch);
        eventBus.Subscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnWasteCaught>(HandleCatch);
        eventBus.Unsubscribe<Events.OnGameRestarted>(HandleRestart);
    }

    private void HandleCatch(Events.OnWasteCaught evt)
    {
        // Here i will set that , so that may be if different waste is having different score
        int points = evt.Waste.GetScore() * Level;

        Score += points;

        // This event will be listened by the gameplay panel to update the score
        eventBus.Publish(new Events.OnScoreAdded(Score));

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

        eventBus.Publish(new Events.OnLevelChanged(Level));
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

        // This event will be listened by the spawner as well as to the ui manager, 
        eventBus.Publish(new Events.OnLevelChanged(Level));
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
