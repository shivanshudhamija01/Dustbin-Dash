using System.Collections;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Lanes")]
    [SerializeField] private CloudLane[] lanes;

    [Header("Cloud Limits")]
    [SerializeField] private int maxClouds = 6;
    [SerializeField] private int maxCloudsPerLane = 2;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 5f;

    [Header("Movement")]
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 50f;

    [Header("Scale Variation")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.5f;

    private int currentClouds;

    private bool spawning;
    private Coroutine spawnRoutine;

    private void OnEnable()
    {
        EventBus.Subscribe<Events.OnGameStarted>(StartSpawner);
        EventBus.Subscribe<Events.OnGameRestarted>(RestartSpawner);
        EventBus.Subscribe<Events.OnGameOver>(GameOver);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnGameStarted>(StartSpawner);
        EventBus.Unsubscribe<Events.OnGameRestarted>(RestartSpawner);
        EventBus.Unsubscribe<Events.OnGameOver>(GameOver);
    }


    public void StartSpawning()
    {
        StopSpawning();

        spawning = true;
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        spawning = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    public void ResetSpawner()
    {
        StopSpawning();

        currentClouds = 0;

        foreach (CloudLane lane in lanes)
        {
            lane.currentClouds = 0;
        }

        CloudPool.Instance.ReturnAll();
    }

    // =====================================================
    // Event Handlers
    // =====================================================

    private void StartSpawner(Events.OnGameStarted evt)
    {
        StartSpawning();
    }

    private void RestartSpawner(Events.OnGameRestarted evt)
    {
        ResetSpawner();
        StartSpawning();
    }

    private void GameOver(Events.OnGameOver evt)
    {
        StopSpawning();
    }

    // =====================================================
    // Spawn Logic
    // =====================================================

    private IEnumerator SpawnRoutine()
    {
        while (spawning)
        {
            if (currentClouds < maxClouds)
            {
                SpawnCloud();
            }

            yield return new WaitForSeconds(
                Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    private void SpawnCloud()
    {
        CloudLane selectedLane = null;

        int safety = 20;

        while (safety > 0)
        {
            CloudLane randomLane =
                lanes[Random.Range(0, lanes.Length)];

            if (randomLane.currentClouds < maxCloudsPerLane)
            {
                selectedLane = randomLane;
                break;
            }

            safety--;
        }

        if (selectedLane == null)
            return;

        bool moveAToB = Random.value > 0.5f;

        Transform spawnPoint =
            moveAToB ? selectedLane.pointA : selectedLane.pointB;

        Transform targetPoint =
            moveAToB ? selectedLane.pointB : selectedLane.pointA;

        GameObject cloud = CloudPool.Instance.GetCloud();

        if (cloud == null)
            return;

        cloud.transform.position = spawnPoint.position;

        float randomScale =
            Random.Range(minScale, maxScale);

        cloud.transform.localScale =
            Vector3.one * randomScale;

        float speed =
            Random.Range(minSpeed, maxSpeed);

        selectedLane.currentClouds++;
        currentClouds++;

        cloud.SetActive(true);

        CloudController controller =
            cloud.GetComponent<CloudController>();

        if (controller != null)
        {
            controller.Initialize(
                targetPoint.position,
                speed,
                CloudPool.Instance,
                this,
                selectedLane);
        }
    }

    public void CloudRemoved(CloudLane lane)
    {
        currentClouds = Mathf.Max(0, currentClouds - 1);

        if (lane != null)
        {
            lane.currentClouds =
                Mathf.Max(0, lane.currentClouds - 1);
        }
    }
}