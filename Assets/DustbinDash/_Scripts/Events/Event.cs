public class Events
{
    public readonly struct OnGameStarted { }
    public readonly struct OnGameRestarted { }
    public readonly struct OnGameOver { }
    public readonly struct OnGamePaused { }
    public readonly struct OnGameResumed { }
    public readonly struct OnWasteCaught
    {
        public readonly WasteItem Waste;

        public OnWasteCaught(WasteItem waste)
        {
            Waste = waste;
        }
    }

    public readonly struct OnWasteMissed
    {
        public readonly WasteItem Waste;

        public OnWasteMissed(WasteItem waste)
        {
            Waste = waste;
        }
    }
    public readonly struct OnWasteWallHit
    {
        public readonly WasteItem Waste;

        public OnWasteWallHit(WasteItem waste)
        {
            Waste = waste;
        }
    }
    public struct OnScoreAdded
    {
        public int Score;
        public OnScoreAdded(int score)
        {
            Score = score;
        }
    }
    public struct OnLevelChanged
    {
        public int Level;
        public OnLevelChanged(int level)
        {
            Level = level;
        }
    }
    public struct OnLivesChanged
    {
        public int Lives;
        public OnLivesChanged(int lives)
        {
            Lives = lives;
        }
    }
    public struct OnGameInput
    {
        public int Direction;
        public OnGameInput(int direction)
        {
            Direction = direction;
        }
    }
}
