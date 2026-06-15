using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WasteItem : MonoBehaviour
{
    [SerializeField] private WasteData config;
    private Rigidbody2D _rb;
    private IEventBus eventBus;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.constraints = RigidbodyConstraints2D.None;

        eventBus = ServiceContainer.Get<IEventBus>();
    }

    private void OnEnable()
    {
        // I will remove these reference from here 
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        eventBus.Subscribe<Events.OnWasteWallHit>(OnWallHit);
    }
    void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnWasteWallHit>(OnWallHit);
    }
    public void Launch(Vector2 velocity, float angularVelocityDeg)
    {
        _rb.constraints = RigidbodyConstraints2D.None;

        _rb.linearVelocity = velocity;
        _rb.angularVelocity = angularVelocityDeg;
    }

    public WasteData GetConfig()
    {
        return config;
    }

    public int GetScore()
    {
        return config.baseScore;
    }
    private void OnWallHit(Events.OnWasteWallHit evt)
    {
        if (evt.Waste != this)
            return;

        if (!config.bounceFromWalls)
            return;

        Vector2 velocity = _rb.linearVelocity;

        velocity.x *= -config.bounceStrength;

        _rb.linearVelocity = velocity;

    }
}