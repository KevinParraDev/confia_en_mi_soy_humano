using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private Vector2 moveDirection;

    private PlayerInput playerInput;
    private CharacterView view;

    private bool movementAllowed = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        view = GetComponentInChildren<CharacterView>();
    }
    public void Initialize()
    {
        AddListeners();
    }
    private void AddListeners()
    {
        moveAction = playerInput.actions["Move"];

        moveAction.performed += BeginAction;
        moveAction.canceled += EndAction;
    }
    private void BeginAction(InputAction.CallbackContext context)
    {
        if (!movementAllowed) return;

        moveDirection = context.ReadValue<Vector2>().normalized;
        view.SetBoolAnimation(Constants.ANIM_MOVING, true);
        view.Turn(moveDirection.x);
    }
    private void EndAction(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
        view.SetBoolAnimation(Constants.ANIM_MOVING, false);
    }
    private void Move()
    {
        rb.linearVelocity = moveDirection * speed;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void RemoveListeners()
    {
        moveAction.performed -= BeginAction;
        moveAction.canceled -= EndAction;
    }

    public void StopMovement()
    {
        movementAllowed = false;
        moveDirection = Vector2.zero;
        view.SetBoolAnimation(Constants.ANIM_MOVING, false);
    }

    public void ResumeMovement()
    {
        movementAllowed = true;

    }

    public void Conclude()
    {
        RemoveListeners();
    }
}
