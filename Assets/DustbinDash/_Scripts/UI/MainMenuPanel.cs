using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button exitButton;

    void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        bgmButton.onClick.AddListener(OnBGMButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void OnPlayButtonClicked()
    {
        EventBus.Publish(new Events.OnGameStarted());
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
