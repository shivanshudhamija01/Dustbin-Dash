using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private int pointsPerLevel = 100;
    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int startingLives = 3;
    [SerializeField] private AudioManager audioManager;
    private IScoreService scoreService;
    private ILivesService livesService;
    private IAudioService audioService;
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

        var livesService = new LivesService();
        ServiceContainer.MapService<ILivesService>(livesService);

        var audioService = new AudioService(audioManager);
        ServiceContainer.MapService<IAudioService>(audioService);
    }
    private void GetReferenceService()
    {
        scoreService = ServiceContainer.Get<IScoreService>();
        livesService = ServiceContainer.Get<ILivesService>();
        audioService = ServiceContainer.Get<IAudioService>();
    }

    private void InitialzeService()
    {
        scoreService.Initialize(pointsPerLevel, maxLevel);
        livesService.Initialize(startingLives);

        // float savedBGM = PlayerPrefs.GetFloat(Keys.BGM, 1f);
        // float savedSFX = PlayerPrefs.GetFloat(Keys.SFX, 1f);

        // audioService.SetBGMVolume(savedBGM);
        // audioService.SetSFXVolume(savedSFX);


        audioService.PlayBGM(SoundType.Bgm);
    }
}
