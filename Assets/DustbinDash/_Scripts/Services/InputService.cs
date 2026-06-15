using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService
{
    private float touchDirection;

    public float GetDirection()
    {
        float keyboardDirection = 0f;

        if (Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.aKey.isPressed)
        {
            keyboardDirection = -1f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed ||
                 Keyboard.current.dKey.isPressed)
        {
            keyboardDirection = 1f;
        }

        return keyboardDirection != 0f
            ? keyboardDirection
            : touchDirection;
    }

    public void SetDirection(float direction)
    {
        touchDirection = direction;
    }
}