using UnityEngine;

public abstract class InteractableBaseController : MonoBehaviour
{
    protected InteractableView view;

    protected virtual void Awake()
    {
        view = GetComponentInChildren<InteractableView>();
    }

    public virtual void Transform(PlayerController playerController = null) { }
    public abstract void Interact(PlayerController playerController = null);
    public abstract void Hover(bool active, PlayerController playerController = null);
}
