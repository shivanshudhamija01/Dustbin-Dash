using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        EventBus.Publish(new Events.OnGameInput(direction));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventBus.Publish(new Events.OnGameInput(0));
    }
}
