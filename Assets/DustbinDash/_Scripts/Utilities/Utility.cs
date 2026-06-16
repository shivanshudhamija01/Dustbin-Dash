using UnityEngine;
[System.Serializable]
public class CloudLane
{
    public Transform pointA;
    public Transform pointB;

    [HideInInspector]
    public int currentClouds;
}
public enum SoundType
{
    Bgm,
    click,
    gameover,
    wastecatch,
    wastedrop
}
[System.Serializable]
public class SoundData
{
    public SoundType soundType;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume = 1f;
}