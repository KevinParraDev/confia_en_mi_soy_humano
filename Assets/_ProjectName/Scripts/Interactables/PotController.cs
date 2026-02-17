using UnityEngine;

public class PotController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private DishesDeskInteractable dishDesk;

    protected override void Awake()
    {
        base.Awake();
        hoverGO?.SetActive(false);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        hoverGO?.SetActive(active);
        view?.Hover(active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            if(playerController.GetCurrentSkin() == NPC.Chef)
            {
                Cook();
            }
            else
            {
                Debug.Log("Solo puedes cocinar si eres chef");
            }
        }
    }
    private void Cook()
    {
        Debug.Log("Cocinar");
        dishDesk.AddDish();
    }
}
