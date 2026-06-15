public interface IScoreService
{
    void Initialize(int pointsPerLevel, int maxLevel);

    int CurrentScore { get; }
    int CurrentLevel { get; }
    int HighScore { get; }

    void AddScore(int points);
    void ResetProgress();
    void SaveHighScore();
}