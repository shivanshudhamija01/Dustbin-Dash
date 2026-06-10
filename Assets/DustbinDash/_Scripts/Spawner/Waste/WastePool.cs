using System.Collections.Generic;
using UnityEngine;
public class WastePool : MonoBehaviour, IWasteProvider
{
    [Header("Pool Setting")]
    [SerializeField] private List<GameObject> wasteItems;
    [SerializeField] private int spawnCountPerItem;

    private readonly List<WasteItem> _availableWaste = new List<WasteItem>();
    private int countOfWasteItems;
    void Awake()
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
    private void OnWasteCaught(Events.OnWasteCaught evt)
    {
        ReturnItem(evt.Waste);
    }

    private void OnWasteMissed(Events.OnWasteMissed evt)
    {
        ReturnItem(evt.Waste);
    }
    private void InstantiatePool()
    {
        for (int i = 0; i < countOfWasteItems; i++)
        {
            for (int j = 0; j < spawnCountPerItem; j++)
            {
                GameObject obj = Instantiate(wasteItems[i], transform);
                WasteItem item = obj.GetComponent<WasteItem>();
                _availableWaste.Add(item);
                obj.SetActive(false);
            }
        }
    }
    public void ReturnItem(WasteItem item)
    {
        item.transform.SetParent(transform);

        item.transform.position = Vector3.zero;
        item.transform.rotation = Quaternion.identity;

        item.gameObject.SetActive(false);

        _availableWaste.Add(item);
    }

    public WasteItem GetWaste()
    {
        if (_availableWaste.Count == 0)
        {
            Debug.LogWarning("Pool is Empty");
            return null;
        }

        int randomIndex = Random.Range(0, _availableWaste.Count);

        WasteItem item = _availableWaste[randomIndex];
        _availableWaste.RemoveAt(randomIndex);
        item.gameObject.SetActive(true);
        return item;
    }

    public void ReturnAll()
    {
        throw new System.NotImplementedException();
    }
}