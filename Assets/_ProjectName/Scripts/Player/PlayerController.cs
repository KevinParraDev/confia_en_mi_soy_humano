using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        movementController?.Initialize();
    }

    public void Conclude()
    {
        movementController?.Conclude();
    }
}
