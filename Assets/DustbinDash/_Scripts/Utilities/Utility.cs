using UnityEngine;
[System.Serializable]
public class CloudLane
{
    public Transform pointA;
    public Transform pointB;

    [HideInInspector]
    public int currentClouds;
}