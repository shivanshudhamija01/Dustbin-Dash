using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button exitButton;
    private IEventBus eventBus;

    void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        bgmButton.onClick.AddListener(OnBGMButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void OnPlayButtonClicked()
    {
        eventBus.Publish(new Events.OnGameStarted());
    }
    void OnBGMButtonClicked()
    {
        // Here i just need to mute and un-mute the audio source and also , just need to 
        // toggle the music icon 
    }
    void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
