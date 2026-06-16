using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image muteIcon;
    [SerializeField] private TextMeshProUGUI bestScore;
    private IEventBus eventBus;
    private IAudioService audioService;
    private bool isMuted = false;
    private IScoreService scoreService;

    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        audioService = ServiceContainer.Get<IAudioService>();
        scoreService = ServiceContainer.Get<IScoreService>();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        bgmButton.onClick.AddListener(OnBGMButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    void Start()
    {
        if (isMuted)
        {
            muteIcon.gameObject.SetActive(true);
            audioService.SetBGMVolume(0);
        }
        else
        {
            muteIcon.gameObject.SetActive(false);
            audioService.SetBGMVolume(1);
        }
    }
    private void OnEnable()
    {
        int highScore = scoreService.HighScore;
        bestScore.text = highScore.ToString();
    }
    void OnPlayButtonClicked()
    {
        audioService.PlaySFX(SoundType.click);
        eventBus.Publish(new Events.OnGameStarted());
    }
    void OnBGMButtonClicked()
    {
        audioService.PlaySFX(SoundType.click);
        if (isMuted)
        {
            muteIcon.gameObject.SetActive(false);
            audioService.SetBGMVolume(1);
            isMuted = false;
        }
        else
        {
            muteIcon.gameObject.SetActive(true);
            audioService.SetBGMVolume(0);
            isMuted = true;
        }
    }
    void OnExitButtonClicked()
    {
        audioService.PlaySFX(SoundType.click);
        Application.Quit();
    }
}
