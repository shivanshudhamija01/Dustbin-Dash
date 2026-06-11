using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WasteSpawner spawner;
    [SerializeField] private MonoBehaviour providerSource;

    private IWasteProvider provider;

    public enum GameState
    {
        Idle,
        Playing,
        GameOver
    }

    public GameState State { get; private set; } = GameState.Idle;

    private void Awake()
    {
        provider = providerSource as IWasteProvider;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<Events.OnGameOver>(HandleGameOver);
        EventBus.Subscribe<Events.OnLevelChanged>(HandleLevelChanged);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnGameOver>(HandleGameOver);
        EventBus.Unsubscribe<Events.OnLevelChanged>(HandleLevelChanged);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (State == GameState.Playing)
            return;

        State = GameState.Playing;

        spawner.StartSpawning(1);

        EventBus.Publish(new Events.OnGameStarted());
    }

    public void RestartGame()
    {
        State = GameState.Idle;

        provider?.ReturnAll();

        EventBus.Publish(new Events.OnGameRestarted());

        StartGame();
    }

    private void HandleGameOver(Events.OnGameOver evt)
    {
        State = GameState.GameOver;

        spawner.StopSpawning();

        provider?.ReturnAll();
    }

    private void HandleLevelChanged(Events.OnLevelChanged evt)
    {
        // spawner.SetLevel(evt.Level);
    }

    // private void Update()
    // {
    //     if (State == GameState.GameOver &&
    //         Input.GetKeyDown(KeyCode.Space))
    //     {
    //         RestartGame();
    //     }
    // }
}