using System.Collections;
using UnityEngine;

/// <summary>
/// Pulls WasteItems from the pool and positions/launches them at timed intervals.
/// Drop this on any persistent GameObject (e.g. GameManager).
/// </summary>
public class WasteSpawner : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    [Header("References")]
    [SerializeField] private WastePool poolReference;

    [Tooltip("All possible waste types. One is chosen randomly each spawn.")]
    [SerializeField] private WasteData[] wasteTypes;

    [Header("Spawn Bounds (world space X)")]
    [SerializeField] private float spawnXMin = -4f;
    [SerializeField] private float spawnXMax = 4f;
    [SerializeField] private float spawnY = 5.5f;   // just above top of camera

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
    private IWastePool _pool;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    private void OnValidate()
    {
        // Sanity-check bounds in the inspector.
        if (spawnXMin >= spawnXMax)
            Debug.LogWarning("[WasteSpawner] spawnXMin must be less than spawnXMax.");
    }

    // ── Public API ────────────────────────────────────────────────────────────
    private void Awake()
    {
        _pool = poolReference;   // WastePool implements IWastePool, so this cast is valid
    }
    void Start()
    {
        StartSpawning();
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
    public void SetLevel(int level)
    {
        _level = Mathf.Max(1, level);
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
        // 1. Borrow from pool ---------------------------------------------------
        Debug.Log("Item is not null ji");
        WasteItem item = _pool.Get();
        if (item == null) return;   // pool denied (growth disabled + pool empty)

        // 2. Pick a random waste type ------------------------------------------
        WasteData data = PickRandomType();
        item.ApplyData(data);

        // 3. Position just above the screen ------------------------------------
        float spawnX = Random.Range(spawnXMin, spawnXMax);
        item.transform.position = new Vector3(spawnX, spawnY, 0f);
        item.transform.rotation = Quaternion.identity;

        // 4. Calculate velocity ------------------------------------------------
        float fallSpeed = CalculateFallSpeed(data);
        float drift = CalculateDrift(data);
        Vector2 velocity = new Vector2(drift, -fallSpeed);

        // 5. Random spin -------------------------------------------------------
        float spin = Random.Range(-maxAngularSpeed, maxAngularSpeed);

        // 6. Launch ------------------------------------------------------------
        item.Launch(velocity, spin);

        // Debug log (disable in release builds)
#if UNITY_EDITOR
        Debug.Log($"[WasteSpawner] Spawned '{data.displayName}' at x={spawnX:F2}  " +
                  $"vy={-fallSpeed:F2}  vx={drift:F2}  spin={spin:F0}°/s  " +
                  $"pool active={_pool.ActiveCount}");
#endif
    }

    // ── Calculation helpers ───────────────────────────────────────────────────

    /// <summary>
    /// Interval between spawns. Shrinks every level, floored at minInterval.
    /// A random jitter is added so spawns never feel mechanical.
    /// </summary>
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
        float speed = baseSpeed
                    + (_level - 1) * speedPerLevel
                    + data.speedBonus
                    + Random.Range(0f, speedJitter);
        return Mathf.Max(0.5f, speed);
    }

    /// <summary>
    /// Horizontal drift: random value in [-max, +max], scaled by level.
    /// </summary>
    private float CalculateDrift(WasteData data)
    {
        float maxDrift = baseDrift + (_level - 1) * driftPerLevel + data.driftBonus;
        return Random.Range(-maxDrift, maxDrift);
    }

    /// <summary>Pick a WasteData uniformly at random.</summary>
    private WasteData PickRandomType()
    {
        if (wasteTypes == null || wasteTypes.Length == 0)
        {
            Debug.LogError("[WasteSpawner] No waste types assigned!");
            return null;
        }
        return wasteTypes[Random.Range(0, wasteTypes.Length)];
    }
}