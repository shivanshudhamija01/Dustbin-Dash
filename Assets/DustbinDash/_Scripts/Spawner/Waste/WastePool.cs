using System.Collections.Generic;
using UnityEngine;

public class WastePool : MonoBehaviour, IWasteProvider
{
    [Header("Pool Setting")]
    [SerializeField] private List<GameObject> wasteItems;

    [SerializeField] private int spawnCountPerItem;

    private readonly List<WasteItem> availableWaste = new();
    private readonly List<WasteItem> activeWaste = new();

    private int countOfWasteItems;

    private void Awake()
    {
        countOfWasteItems = wasteItems.Count;
        InstantiatePool();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<Events.OnWasteCaught>(OnWasteCaught);
        EventBus.Subscribe<Events.OnWasteMissed>(OnWasteMissed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Events.OnWasteCaught>(OnWasteCaught);
        EventBus.Unsubscribe<Events.OnWasteMissed>(OnWasteMissed);
    }

    private void InstantiatePool()
    {
        for (int i = 0; i < countOfWasteItems; i++)
        {
            for (int j = 0; j < spawnCountPerItem; j++)
            {
                GameObject obj =
                    Instantiate(wasteItems[i], transform);

                WasteItem item =
                    obj.GetComponent<WasteItem>();

                obj.SetActive(false);

                availableWaste.Add(item);
            }
        }
    }

    public WasteItem GetWaste()
    {
        if (availableWaste.Count == 0)
        {
            Debug.LogWarning("Pool Empty");
            return null;
        }

        int randomIndex =
            Random.Range(0, availableWaste.Count);

        WasteItem item =
            availableWaste[randomIndex];

        availableWaste.RemoveAt(randomIndex);

        activeWaste.Add(item);

        item.gameObject.SetActive(true);

        return item;
    }

    public void ReturnItem(WasteItem item)
    {
        if (item == null)
            return;

        activeWaste.Remove(item);

        item.transform.SetParent(transform);

        item.transform.position = Vector3.zero;
        item.transform.rotation = Quaternion.identity;

        item.gameObject.SetActive(false);

        availableWaste.Add(item);
    }

    private void OnWasteCaught(Events.OnWasteCaught evt)
    {
        ReturnItem(evt.Waste);
    }

    private void OnWasteMissed(Events.OnWasteMissed evt)
    {
        ReturnItem(evt.Waste);
    }

    public void ReturnAll()
    {
        while (activeWaste.Count > 0)
        {
            ReturnItem(activeWaste[0]);
        }
    }
}