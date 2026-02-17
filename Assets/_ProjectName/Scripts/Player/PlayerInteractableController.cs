using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractableController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InteractableBaseController interactableInRange;
    [SerializeField] private PlayerCanvasController playerCanvasController;
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
            SoundManager.Instance.PlaySFXByName(Constants.SFX_CLICK);
            interactableInRange.Interact(playerController);
        }
    }
    public void Transform(InputAction.CallbackContext context)
    {
        if (interactableInRange != null)
        {
            interactableInRange.Transform(playerController);
            if (interactableInRange is NpcInteractableController)
            {
                SoundManager.Instance.PlaySFXByName(Constants.SFX_ACTION_1);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_WOOSH_1);
                playerCanvasController?.PlayChangeSkinAnimation();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactableInRange == null && collision.TryGetComponent<InteractableBaseController>(out interactableInRange))
        {
            if (interactableInRange is NpcInteractableController npc)
            {
                playerCanvasController?.SetActiveButtons(true, npc.GetData().maskIcon);
            }
            interactableInRange.Hover(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<InteractableBaseController>(out InteractableBaseController exitInteractable) && interactableInRange == exitInteractable)
        {
            if (interactableInRange is NpcInteractableController npc)
            {
                playerCanvasController?.SetActiveButtons(false, npc.GetData().maskIcon);
            }
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
