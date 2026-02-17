using UnityEngine;

public class DishController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private bool poison = false;
    protected override void Awake()
    {
        base.Awake();
        hoverGO.SetActive(false);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        view?.Hover(active);
        hoverGO.SetActive(active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Waiter)
            {
                Debug.Log("Tomar plato");
            }
            else
            {
                Debug.Log("Solo puedes llevar platos si eres mesero");
            }
        }
    }
    public override void Transform(PlayerController playerController = null)
    {
        base.Transform();
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Waiter)
            {
                poison = true;
                Debug.Log("Envenenar");
            }
            else
            {
                Debug.Log("No eres mesero");
            }
        }
    }
}

