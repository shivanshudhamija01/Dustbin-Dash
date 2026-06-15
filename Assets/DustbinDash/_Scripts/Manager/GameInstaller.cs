using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private int pointsPerLevel = 100;
    [SerializeField] private int maxLevel = 10;
    private IScoreService scoreService;

    private void Awake()
    {
        RegisterServices();
        GetReferenceService();
        InitialzeService();
    }


    void RegisterServices()
    {
        var eventBus = new EventBus();
        ServiceContainer.MapService<IEventBus>(eventBus);

        var inputService = new InputService();
        ServiceContainer.MapService<IInputService>(inputService);

        var scoreService = new ScoreService();
        ServiceContainer.MapService<IScoreService>(scoreService);
    }
    private void GetReferenceService()
    {
        scoreService = ServiceContainer.Get<IScoreService>();
    }

    private void InitialzeService()
    {
        scoreService.Initialize(pointsPerLevel, maxLevel);
    }
}
