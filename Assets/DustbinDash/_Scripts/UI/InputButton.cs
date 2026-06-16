using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int direction;

    private IInputService inputService;
    private void Awake()
    {
        inputService = ServiceContainer.Get<IInputService>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        inputService.SetDirection(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputService.SetDirection(0);
    }
}
