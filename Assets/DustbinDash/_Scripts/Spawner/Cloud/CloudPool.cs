using System.Collections.Generic;
using UnityEngine;

public class CloudPool : MonoBehaviour
{
    public static CloudPool Instance;

    [Header("Cloud Prefabs")]
    [SerializeField] private GameObject[] cloudPrefabs;

    [Header("Pool Settings")]
    [SerializeField] private int poolSizePerCloud = 5;

    private List<GameObject> pooledClouds =
        new List<GameObject>();

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

                pooledClouds.Add(cloud);
            }
        }
    }

    public GameObject GetCloud()
    {
        List<GameObject> inactiveClouds =
            new List<GameObject>();

        foreach (GameObject cloud in pooledClouds)
        {
            if (!cloud.activeInHierarchy)
            {
                inactiveClouds.Add(cloud);
            }
        }

        if (inactiveClouds.Count == 0)
            return null;

        int randomIndex =
            Random.Range(0, inactiveClouds.Count);

        return inactiveClouds[randomIndex];
    }

    public void ReturnCloud(GameObject cloud)
    {
        cloud.SetActive(false);
    }

    public void ReturnAll()
    {
        foreach (GameObject cloud in pooledClouds)
        {
            if (cloud.activeInHierarchy)
            {
                cloud.SetActive(false);
            }
        }
    }
}