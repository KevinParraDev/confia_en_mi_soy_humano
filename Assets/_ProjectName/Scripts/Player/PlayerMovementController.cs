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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
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
        moveDirection = context.ReadValue<Vector2>().normalized;
    }
    private void EndAction(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
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
    public void Conclude()
    {
        RemoveListeners();
    }
}
