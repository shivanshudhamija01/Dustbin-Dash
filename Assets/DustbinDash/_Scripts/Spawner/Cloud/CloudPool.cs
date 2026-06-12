using System.Collections.Generic;
using UnityEngine;

public class CloudPool : MonoBehaviour
{
    public static CloudPool Instance;

    [Header("Cloud Prefabs")]
    [SerializeField] private GameObject[] cloudPrefabs;

    [Header("Pool Settings")]
    [SerializeField] private int poolSizePerCloud = 5;

    private readonly List<GameObject> availableClouds = new();
    private readonly List<GameObject> activeClouds = new();

    private void Awake()
    {
        Instance = this;
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < cloudPrefabs.Length; i++)
        {
            for (int j = 0; j < poolSizePerCloud; j++)
            {
                GameObject cloud =
                    Instantiate(cloudPrefabs[i], transform);

                cloud.SetActive(false);

                availableClouds.Add(cloud);
            }
        }
    }

    public GameObject GetCloud()
    {
        if (availableClouds.Count == 0)
            return null;

        int randomIndex =
            Random.Range(0, availableClouds.Count);

        GameObject cloud = availableClouds[randomIndex];

        availableClouds.RemoveAt(randomIndex);
        activeClouds.Add(cloud);

        return cloud;
    }

    public void ReturnCloud(GameObject cloud)
    {
        if (cloud == null)
            return;

        activeClouds.Remove(cloud);

        cloud.transform.SetParent(transform);
        cloud.SetActive(false);

        availableClouds.Add(cloud);
    }

    public void ReturnAll()
    {
        while (activeClouds.Count > 0)
        {
            ReturnCloud(activeClouds[0]);
        }
    }
}