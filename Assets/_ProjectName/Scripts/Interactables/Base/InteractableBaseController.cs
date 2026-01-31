using UnityEngine;

public abstract class InteractableBaseController : MonoBehaviour
{
    public abstract void Interact(PlayerController playerController = null);
    public abstract void Hover(bool active, PlayerController playerController = null);
}
