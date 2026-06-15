using UnityEngine;

public interface IScoreService
{
    void Initialize(int pointsPerLevel, int maxLevel);
    void SetScore(int score);
    void SetHighScore(int score);
    int GetCurrentScore();
    int GetHighScore();
}
