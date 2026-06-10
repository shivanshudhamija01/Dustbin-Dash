using UnityEngine;
public class ItemDetector : MonoBehaviour
{
    private WasteItem wasteItem;

    private void Awake()
    {
        wasteItem = GetComponent<WasteItem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BinOpening"))
        {
            EventBus.Publish(new Events.OnWasteCaught(wasteItem));
        }

        if (other.CompareTag("Ground"))
        {
            EventBus.Publish(
                new Events.OnWasteMissed(wasteItem));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            EventBus.Publish(
                new Events.OnWasteWallHit(wasteItem));
        }
    }
}