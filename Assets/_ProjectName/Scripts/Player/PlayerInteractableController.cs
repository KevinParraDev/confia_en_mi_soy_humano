using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractableController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InteractableBaseController interactableInRange;
    private InputAction interactAction;
    private InputAction transformAction;

    private void Awake()
    {
        interactAction = GetComponentInParent<PlayerInput>().actions["Interact"];
        transformAction = GetComponentInParent<PlayerInput>().actions["Transform"];
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        AddListeners();
    }
    private void AddListeners()
    {
        interactAction.performed += Interact;
        transformAction.performed += Transform;
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (interactableInRange != null)
        {
            interactableInRange.Interact(playerController);
        }
    }
    public void Transform(InputAction.CallbackContext context)
    {
        if (interactableInRange != null)
        {
            interactableInRange.Transform(playerController);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactableInRange == null && collision.TryGetComponent<InteractableBaseController>(out interactableInRange))
        {
            interactableInRange.Hover(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<InteractableBaseController>(out InteractableBaseController exitInteractable) && interactableInRange == exitInteractable)
        {
            interactableInRange.Hover(false);
            interactableInRange = null;
        }
    }
    private void RemoveListeners()
    {
        interactAction.performed -= Interact;
        transformAction.performed -= Transform;
    }
    public void Conclude()
    {
        RemoveListeners();
    }
}
