public class Events
{
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
    public readonly struct OnGameRestarted
    {

    }
    public readonly struct OnScoreAdded
    {
        public readonly int points;
        public OnScoreAdded(int points)
        {
            this.points = points;
        }
    }
    public readonly struct OnLevelChanged
    {
        public readonly int Level;
        public OnLevelChanged(int level)
        {
            this.Level = level;
        }
    }
    public readonly struct OnLivesChanged
    {
        public readonly int lives;
        public OnLivesChanged(int lives)
        {
            this.lives = lives;
        }
    }
    public readonly struct OnGameOver
    {

    }
    public readonly struct OnGameStarted
    {

    }
}
