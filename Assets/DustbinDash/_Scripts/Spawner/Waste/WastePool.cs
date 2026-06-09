using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Concrete object pool that implements IWastePool.
/// WasteItem and WasteSpawner talk to IWastePool only —
/// they never reference this class directly.
/// </summary>
public class WastePool : MonoBehaviour
{
    // [Header("Pool Settings")]
    // [SerializeField] private GameObject wastePrefab;

    // [Tooltip("Number of instances pre-warmed at startup.")]
    // [SerializeField] private int initialSize = 20;

    // [Tooltip("Grow automatically when the pool runs dry.")]
    // [SerializeField] private bool allowGrowth = true;

    // // ── Internal state ───────────────────────────────────────────────────────

    // private readonly Queue<WasteItem> _available = new Queue<WasteItem>();
    // private readonly List<WasteItem> _all = new List<WasteItem>();

    // // ── Lifecycle ────────────────────────────────────────────────────────────

    // private void Awake()
    // {
    //     Prewarm(initialSize);
    // }

    // // ── IWastePool implementation ────────────────────────────────────────────

    // public WasteItem Get()
    // {
    //     if (_available.Count == 0)
    //     {
    //         if (allowGrowth)
    //             CreateInstance();
    //         else
    //         {
    //             Debug.LogWarning("[WastePool] Pool is empty and growth is disabled.");
    //             return null;
    //         }
    //     }

    //     WasteItem item = _available.Dequeue();
    //     item.gameObject.SetActive(true);
    //     return item;
    // }

    // public void Return(WasteItem item)
    // {
    //     if (item == null) return;

    //     item.gameObject.SetActive(false);
    //     item.transform.SetParent(transform);
    //     _available.Enqueue(item);
    // }

    // public void ReturnAll()
    // {
    //     foreach (WasteItem item in _all)
    //     {
    //         if (item.gameObject.activeSelf)
    //             Return(item);
    //     }
    // }

    // public int ActiveCount => _all.Count - _available.Count;
    // public int TotalCount => _all.Count;

    // // ── Private helpers ──────────────────────────────────────────────────────

    // private void Prewarm(int count)
    // {
    //     for (int i = 0; i < count; i++)
    //         CreateInstance();
    // }

    // private void CreateInstance()
    // {
    //     GameObject go = Instantiate(wastePrefab, transform);
    //     WasteItem item = go.GetComponent<WasteItem>() ?? go.AddComponent<WasteItem>();

    //     // Inject THIS pool as the IWastePool — WasteItem never sees the concrete type.
    //     item.Init(this);

    //     go.SetActive(false);
    //     _available.Enqueue(item);
    //     _all.Add(item);
    // }
    public static WastePool Instance;

    [Header("Pool Setting")]
    [SerializeField] private List<GameObject> wasteItems;
    [SerializeField] private int spawnCountPerItem;

    private List<GameObject> _availableWaste = new List<GameObject>();
    private int countOfWasteItems;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        countOfWasteItems = wasteItems.Count;
        InstantiatePool();
    }
    private void InstantiatePool()
    {
        for (int i = 0; i < countOfWasteItems; i++)
        {
            for (int j = 0; j < spawnCountPerItem; j++)
            {
                GameObject obj = Instantiate(wasteItems[i], transform);
                _availableWaste.Add(obj);
                obj.SetActive(false);
            }
        }
    }
    // It will return the waste item to the spawner 
    public GameObject GetItem()
    {
        if (_availableWaste.Count == 0)
        {
            Debug.LogWarning("Pool Empty");
            return null;
        }

        int randomIndex = Random.Range(0, _availableWaste.Count);

        GameObject item = _availableWaste[randomIndex];

        _availableWaste.RemoveAt(randomIndex);
        item.SetActive(true);
        return item;
    }
    public void ReturnItem(GameObject item)
    {
        item.transform.SetParent(transform);

        item.transform.position = Vector3.zero;
        item.transform.rotation = Quaternion.identity;

        item.SetActive(false);

        _availableWaste.Add(item);
    }

}