using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private RectTransform spawnPosition;

    private RectTransform playerRect;
    private Vector2 spawnAnchoredPosition;
    private IEventBus eventBus;
    private void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        playerRect = playerObject.GetComponent<RectTransform>();

        // Cache the spawn position once
        spawnAnchoredPosition = spawnPosition.anchoredPosition;
    }

    private void OnEnable()
    {
        eventBus.Subscribe<Events.OnGameStarted>(SpawnPlayer);
        eventBus.Subscribe<Events.OnGameRestarted>(ResetPlayer);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnGameStarted>(SpawnPlayer);
        eventBus.Unsubscribe<Events.OnGameRestarted>(ResetPlayer);
    }

    private void SpawnPlayer(Events.OnGameStarted evt)
    {
        ResetPosition();
        ActivatePlayer();
    }

    private void ResetPlayer(Events.OnGameRestarted evt)
    {
        ResetPosition();
        ActivatePlayer();
    }

    private void ActivatePlayer()
    {
        playerObject.SetActive(true);
    }

    private void ResetPosition()
    {
        playerRect.anchoredPosition = spawnAnchoredPosition;
        playerRect.localRotation = Quaternion.identity;
        playerRect.localScale = Vector3.one;
    }
}