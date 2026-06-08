using UnityEngine;

/// <summary>
/// Attached to every waste prefab.
/// Holds a reference to IWastePool — knows nothing about the concrete WastePool class.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WasteItem : MonoBehaviour
{
    // ── Runtime data (set externally) ────────────────────────────────────────

    [HideInInspector] public WasteData Data;

    [Header("Cull Y — item is returned to pool below this world-Y")]
    [SerializeField] private float cullY = -6f;

    // ── Private refs ─────────────────────────────────────────────────────────

    private IWastePool _pool;   // <-- interface, not WastePool
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    // ── Events ───────────────────────────────────────────────────────────────

    public static event System.Action<WasteItem> OnMissed;
    public static event System.Action<WasteItem> OnCaught;

    // ── Lifecycle ────────────────────────────────────────────────────────────

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _rb.gravityScale = 0f;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnEnable()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

    private void Update()
    {
        if (transform.position.y < cullY)
        {
            OnMissed?.Invoke(this);
            ReturnToPool();
        }
    }

    // ── Public API ───────────────────────────────────────────────────────────

    /// <summary>
    /// Called once by WastePool right after instantiation.
    /// Stores the pool reference as IWastePool — no concrete type needed.
    /// </summary>
    public void Init(IWastePool pool)
    {
        _pool = pool;
    }

    public void Launch(Vector2 velocity, float angularVelocityDeg)
    {
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

    // ── Private helpers ──────────────────────────────────────────────────────

    private void ReturnToPool()
    {
        _pool?.Return(this);
    }

    // ── Collision ────────────────────────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BinOpening"))
            Catch();
    }
}