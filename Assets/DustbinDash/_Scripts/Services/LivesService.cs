using UnityEngine;

public class LivesService : ILivesService
{
    private int currentLives;
    private int startingLives;

    public int CurrentLives => currentLives;

    public void Initialize(int startingLives)
    {
        this.startingLives = startingLives;
        ResetLives();
    }

    public void LoseLife()
    {
        currentLives = Mathf.Max(0, currentLives - 1);
    }

    public void ResetLives()
    {
        currentLives = startingLives;
    }
}