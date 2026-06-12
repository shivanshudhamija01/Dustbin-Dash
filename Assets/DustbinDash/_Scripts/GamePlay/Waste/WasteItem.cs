using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WasteItem : MonoBehaviour
{

    [SerializeField] private WasteData config;
    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // i will remove these from here and add it to the waste pool while resetting it to the pool
        _rb.gravityScale = 0f;
        _rb.constraints = RigidbodyConstraints2D.None;
    }

    private void OnEnable()
    {
        // I will remove these reference from here 
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        EventBus.Subscribe<Events.OnWasteWallHit>(OnWallHit);
    }
    void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnWasteWallHit>(OnWallHit);
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