using UnityEngine;
using UnityEngine.InputSystem;

public class DustbinController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 800f;
    [SerializeField] private float edgePadding = 30f;
    [SerializeField] private Animator animator;
    private RectTransform rectTransform;
    private RectTransform canvasRect;
    private float input;
    private int isWalking;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        Canvas canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }
    void Start()
    {
        isWalking = Animator.StringToHash("isWalking");
    }
    private void Update()
    {
        HandleInput();
        Move();
    }

    private void HandleInput()
    {
        input = 0f;

        if (Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.aKey.isPressed)
        {
            input = -1f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed ||
                 Keyboard.current.dKey.isPressed)
        {
            input = 1f;
        }
        bool walkInput = input != 0 ? true : false;
        animator.SetBool(isWalking, walkInput);
    }

    private void Move()
    {
        Vector2 pos = rectTransform.anchoredPosition;

        pos.x += input * moveSpeed * Time.deltaTime;

        float canvasHalfWidth = canvasRect.rect.width * 0.5f;
        float binHalfWidth = rectTransform.rect.width * 0.5f;

        float minX = -canvasHalfWidth + binHalfWidth + edgePadding;
        float maxX = canvasHalfWidth - binHalfWidth - edgePadding;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        rectTransform.anchoredPosition = pos;
    }
}