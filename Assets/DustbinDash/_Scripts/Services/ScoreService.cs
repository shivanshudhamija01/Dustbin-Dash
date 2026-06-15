using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ScoreService : IScoreService
{
    private int pointsPerLevel;
    private int maxLevel;
    public int GetCurrentScore()
    {
        return 0;
    }

    public int GetHighScore()
    {
        return 0;
    }

    public void Initialize(int pointsPerLevel, int maxLevel)
    {
        this.pointsPerLevel = pointsPerLevel;
        this.maxLevel = maxLevel;
    }

    public void SetHighScore(int score)
    {

    }

    public void SetScore(int score)
    {

    }
}
