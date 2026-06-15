using UnityEngine;

public class ScoreService : IScoreService
{
    private int currentScore;
    private int currentLevel;
    private int highScore;

    private int nextLevelThreshold;
    private int pointsPerLevel;
    private int maxLevel;

    public int CurrentScore => currentScore;
    public int CurrentLevel => currentLevel;
    public int HighScore => highScore;

    public void Initialize(int pointsPerLevel, int maxLevel)
    {
        this.pointsPerLevel = pointsPerLevel;
        this.maxLevel = maxLevel;

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        ResetProgress();
    }

    public void AddScore(int points)
    {
        currentScore += points;

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentLevel >= maxLevel)
            return;

        if (currentScore < nextLevelThreshold)
            return;

        currentLevel++;

        nextLevelThreshold += pointsPerLevel;
    }

    public void ResetProgress()
    {
        currentScore = 0;
        currentLevel = 1;
        nextLevelThreshold = pointsPerLevel;
    }

    public void SaveHighScore()
    {
        if (currentScore <= highScore)
            return;

        highScore = currentScore;

        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }
}