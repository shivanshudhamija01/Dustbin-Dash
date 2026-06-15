using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
public class ItemDetector : MonoBehaviour
{
    private WasteItem wasteItem;
    private IEventBus eventBus;
    private IAudioService audioService;
    private void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        audioService = ServiceContainer.Get<IAudioService>();
        wasteItem = GetComponent<WasteItem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BinOpening"))
        {
            audioService.PlaySFX(SoundType.wastecatch);
            eventBus.Publish(new Events.OnWasteCaught(wasteItem));
        }

        if (other.CompareTag("Ground"))
        {
            audioService.PlaySFX(SoundType.wastedrop);
            eventBus.Publish(new Events.OnWasteMissed(wasteItem));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            eventBus.Publish(new Events.OnWasteWallHit(wasteItem));
        }
    }
}