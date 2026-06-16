using UnityEngine;

public class DustbinController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 800f;
    [SerializeField] private float edgePadding = 30f;
    [SerializeField] private Animator animator;

    private RectTransform rectTransform;
    private RectTransform canvasRect;

    private IInputService inputService;

    private float input;
    private int isWalking;
    private int openBin;
    private IEventBus eventBus;
    private void Awake()
    {
        eventBus = ServiceContainer.Get<IEventBus>();
        rectTransform = GetComponent<RectTransform>();

        Canvas canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();

        inputService = ServiceContainer.Get<IInputService>();
    }

    private void Start()
    {
        isWalking = Animator.StringToHash("isWalking");
        openBin = Animator.StringToHash("OpenBin");
    }
    void OnEnable()
    {
        eventBus.Subscribe<Events.OnBinOpenRequested>(OpenBinAndCatchWaste);
        eventBus.Subscribe<Events.OnGameRestarted>(ResetAnimator);

    }
    void OnDisable()
    {
        eventBus.Unsubscribe<Events.OnBinOpenRequested>(OpenBinAndCatchWaste);
        eventBus.Unsubscribe<Events.OnGameRestarted>(ResetAnimator);

    }
    private void Update()
    {
        input = inputService.GetDirection();

        animator.SetBool(isWalking, input != 0f);

        Move();
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
    void OpenBinAndCatchWaste(Events.OnBinOpenRequested evt)
    {
        animator.SetTrigger(openBin);
    }
    private void ResetAnimator(Events.OnGameRestarted evt)
    {
        animator.ResetTrigger(openBin);
        animator.SetBool(isWalking, false);

        animator.Play("Idle", 0, 0f);
    }
}
