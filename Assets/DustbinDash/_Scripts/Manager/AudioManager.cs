using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource gui;

    [SerializeField] private List<SoundData> soundMaps;

    public AudioSource BGMSource => bgm;
    public AudioSource SFXSource => sfx;
    public AudioSource GUISource => gui;

    public List<SoundData> Audios => soundMaps;
}