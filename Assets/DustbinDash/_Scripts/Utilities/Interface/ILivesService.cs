public interface ILivesService
{
    void Initialize(int startingLives);

    int CurrentLives { get; }

    void LoseLife();

    void ResetLives();
}