using System.Collections;
using UnityEngine;

/// <summary>
/// Pulls WasteItems from the pool and positions/launches them at timed intervals.
/// Drop this on any persistent GameObject (e.g. GameManager).
/// </summary>
public class WasteSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MonoBehaviour providerSource;
    [SerializeField] private RectTransform spawnArea;


    [Header("Base Interval (seconds between spawns)")]
    [SerializeField] private float baseInterval = 1.8f;
    [SerializeField] private float intervalJitter = 0.5f;   // ± random added to each interval


    [Header("Fall Speed")]
    [SerializeField] private float baseSpeed = 2.5f;
    [SerializeField] private float speedPerLevel = 0.4f;
    [SerializeField] private float speedJitter = 0.6f;   // random extra speed per item


    [Header("Horizontal Drift")]
    [SerializeField] private float baseDrift = 0.6f;   // max |vx| at level 1
    [SerializeField] private float driftPerLevel = 0.08f;


    [Header("Rotation")]
    [SerializeField] private float maxAngularSpeed = 120f;   // degrees/sec

    [Header("Level Scaling")]
    [Tooltip("Minimum interval the spawner will ever reach, regardless of level.")]
    [SerializeField] private float minInterval = 0.35f;

    [Tooltip("Each level reduces the interval by this many seconds.")]
    [SerializeField] private float intervalReduction = 0.12f;

    // ── Private state ─────────────────────────────────────────────────────────

    private int _level = 1;
    private bool _spawning = false;
    private Coroutine _spawnRoutine;
    private IWasteProvider provider;
    private void Awake()
    {
        provider = providerSource as IWasteProvider;
    }
    void OnEnable()
    {
        EventBus.Subscribe<Events.OnLevelChanged>(SetLevel);
        EventBus.Subscribe<Events.OnGameStarted>(StartSpawner);
        EventBus.Subscribe<Events.OnGameRestarted>(ResetSpawner);
        EventBus.Subscribe<Events.OnGameOver>(GameOver);
    }
    void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnLevelChanged>(SetLevel);
        EventBus.Unsubscribe<Events.OnGameStarted>(StartSpawner);
        EventBus.Unsubscribe<Events.OnGameRestarted>(ResetSpawner);
        EventBus.Unsubscribe<Events.OnGameOver>(GameOver);
    }

    /// <summary>Start spawning from the given level (default 1).</summary>
    public void StartSpawning(int level = 1)
    {
        StopSpawning();
        _level = Mathf.Max(1, level);
        _spawning = true;
        _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    /// <summary>Pause spawning (e.g. game paused or game over).</summary>
    public void StopSpawning()
    {
        _spawning = false;
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }
    /// <summary>Increase difficulty by one level.</summary>
    /// I have to invoke this method on score update after particular range
    public void SetLevel(Events.OnLevelChanged evt)
    {
        int level = evt.Level;
        _level = Mathf.Max(1, level);
        Debug.Log("Level changed he he : " + level);
    }

    // ── Core spawn loop ───────────────────────────────────────────────────────

    private IEnumerator SpawnLoop()
    {
        // Small initial delay so the player has a moment to get ready.
        yield return new WaitForSeconds(0.8f);

        while (_spawning)
        {
            SpawnOne();
            yield return new WaitForSeconds(CalculateInterval());
        }
    }

    private void SpawnOne()
    {
        WasteItem item = provider.GetWaste();
        if (item == null) return;

        // 2. Pick a random waste type ------------------------------------------
        WasteData data = item.GetConfig();

        // 3. Position just above the screen ------------------------------------
        Vector3 spawnPos = GetSpawnPosition();

        item.transform.position = spawnPos;
        item.transform.rotation = Quaternion.identity;

        // 4. Calculate velocity ------------------------------------------------
        float fallSpeed = CalculateFallSpeed(data);
        float drift = CalculateDrift(data);
        Vector2 velocity = new Vector2(drift, -fallSpeed);

        // 5. Random spin -------------------------------------------------------
        // May be here i can also add multipler like i did in speed and all that things from the wastedata
        float spin = Random.Range(-maxAngularSpeed, maxAngularSpeed) * data.angularVelocityMultiplier;

        // 6. Launch ------------------------------------------------------------
        item.Launch(velocity, spin);

    }

    private float CalculateInterval()
    {
        float interval = baseInterval - (_level - 1) * intervalReduction;
        interval = Mathf.Max(interval, minInterval);
        interval += Random.Range(-intervalJitter * 0.5f, intervalJitter);
        return Mathf.Max(0.1f, interval);
    }

    /// <summary>
    /// Fall speed = base + level scaling + per-type bonus + random jitter.
    /// </summary>
    private float CalculateFallSpeed(WasteData data)
    {
        float speed = (baseSpeed + (_level - 1) * speedPerLevel + Random.Range(0f, speedJitter)) * data.speedMultiplier;
        Debug.Log("Level is this : " + _level + " " + "speed is : " + speed);
        return Mathf.Max(0.5f, speed);
    }

    /// <summary>
    /// Horizontal drift: random value in [-max, +max], scaled by level.
    /// </summary>
    private float CalculateDrift(WasteData data)
    {
        float maxDrift = (baseDrift + (_level - 1) * driftPerLevel) * data.driftMultiplier;
        return Random.Range(-maxDrift, maxDrift);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3[] corners = new Vector3[4];
        spawnArea.GetWorldCorners(corners);

        return new Vector3(
            Random.Range(corners[0].x, corners[3].x),
            spawnArea.position.y,
            0f
        );
    }
    private void StartSpawner(Events.OnGameStarted evt)
    {
        StartSpawning();
    }
    private void ResetSpawner(Events.OnGameRestarted evt)
    {
        StopSpawning();
        provider.ReturnAll();
        StartSpawning();
    }
    private void GameOver(Events.OnGameOver evt)
    {
        StopSpawning();
    }
}
// Spanwer will listen to the event , 
// When the play game is start or when the restart is pressed, 
// First think he will do is that, stop spawning , and then start spawning
// And also reset the pool 