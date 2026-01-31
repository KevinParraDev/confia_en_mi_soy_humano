using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController movementController;
    private PlayerInteractableController interactableController;

    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        interactableController = GetComponentInChildren<PlayerInteractableController>();
    }
    public void Initialize()
    {
        movementController?.Initialize();
        interactableController?.Initialize();
    }

    public void Conclude()
    {
        movementController?.Conclude();
        interactableController?.Conclude();
    }
}
