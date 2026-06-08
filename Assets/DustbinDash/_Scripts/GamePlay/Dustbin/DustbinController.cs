using UnityEngine;

public class DustbinController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 800f;

    [SerializeField] private float leftLimit = -800f;
    [SerializeField] private float rightLimit = 800f;

    private RectTransform rectTransform;

    private float moveInput;
    private float input;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        input = 0;

        if (UnityEngine.InputSystem.Keyboard.current.leftArrowKey.isPressed ||
            UnityEngine.InputSystem.Keyboard.current.aKey.isPressed)
        {
            input = -1;
        }
        else if (UnityEngine.InputSystem.Keyboard.current.rightArrowKey.isPressed ||
                 UnityEngine.InputSystem.Keyboard.current.dKey.isPressed)
        {
            input = 1;
        }
        Vector2 pos = rectTransform.anchoredPosition;

        pos.x += moveInput * moveSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);

        rectTransform.anchoredPosition = pos;
    }

    public void MoveLeft()
    {
        moveInput = -1f;
    }

    public void MoveRight()
    {
        moveInput = 1f;
    }

    public void StopMoving()
    {
        moveInput = 0f;
    }
}
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class DustbinController : MonoBehaviour
// {
//     [SerializeField] private float moveSpeed = 800f;

//     [SerializeField] private float leftLimit = -800f;
//     [SerializeField] private float rightLimit = 800f;

//     private RectTransform rectTransform;
//     private float input;
//     private void Awake()
//     {
//         rectTransform = GetComponent<RectTransform>();
//     }

//     private void Update()
//     {
//         input = 0;

//         if (UnityEngine.InputSystem.Keyboard.current.leftArrowKey.isPressed ||
//             UnityEngine.InputSystem.Keyboard.current.aKey.isPressed)
//         {
//             input = -1;
//         }
//         else if (UnityEngine.InputSystem.Keyboard.current.rightArrowKey.isPressed ||
//                  UnityEngine.InputSystem.Keyboard.current.dKey.isPressed)
//         {
//             input = 1;
//         }
//         // float input = 1;
//         Vector3 position = transform.position;

//         position.x += input * moveSpeed * Time.deltaTime;

//         position.x = Mathf.Clamp(
//             position.x,
//             leftLimit,
//             rightLimit);

//         transform.position = position;
//     }
// }