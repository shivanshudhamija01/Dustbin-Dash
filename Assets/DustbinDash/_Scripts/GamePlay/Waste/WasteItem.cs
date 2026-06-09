using UnityEngine;

/// <summary>
/// Attached to every waste prefab.
/// Holds a reference to IWastePool — knows nothing about the concrete WastePool class.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WasteItem : MonoBehaviour
{
    // ── Runtime data ──────────────────────────────────────────────────────────

    [HideInInspector] public WasteData Data;

    // ── Private refs ──────────────────────────────────────────────────────────

    private IWastePool _pool;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    // ── Events ────────────────────────────────────────────────────────────────

    public static event System.Action<WasteItem> OnMissed;
    public static event System.Action<WasteItem> OnCaught;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();

        _rb.gravityScale = 0f;

        // FIX 1: Remove FreezeRotation — this was blocking angularVelocity entirely.
        // FIX 2: Only freeze X and Y position constraints, never rotation.
        _rb.constraints = RigidbodyConstraints2D.None;
    }

    private void OnEnable()
    {
        // FIX 3: Reset velocity here is fine ONLY because OnEnable fires
        // before Launch() is called by the spawner. But to be safe, we reset
        // both linear and angular so the item is clean when borrowed from pool.
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void Init(IWastePool pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// Apply velocity and spin.
    /// velocity.x = horizontal drift (left or right)
    /// velocity.y = fall speed (always negative, downward)
    /// angularVelocityDeg = degrees/sec spin
    /// </summary>
    public void Launch(Vector2 velocity, float angularVelocityDeg)
    {
        // FIX 4: Force constraints to None right before applying velocity —
        // guarantees no stale constraint blocks movement on re-use from pool.
        _rb.constraints = RigidbodyConstraints2D.None;

        _rb.linearVelocity = velocity;
        _rb.angularVelocity = angularVelocityDeg;
    }

    public void ApplyData(WasteData data)
    {
        Data = data;
        if (_sr != null && data.sprite != null)
            _sr.sprite = data.sprite;
    }

    public void Catch()
    {
        OnCaught?.Invoke(this);
        ReturnToPool();
    }

    // ── Private ───────────────────────────────────────────────────────────────

    private void ReturnToPool()
    {
        WastePool.Instance.ReturnItem(this.gameObject);
    }

    private void Drop()
    {
        OnMissed?.Invoke(this);
        ReturnToPool();
    }

    // ── Collision ─────────────────────────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BinOpening"))
            Catch();

        if (other.CompareTag("Ground"))
            Drop();
    }
}