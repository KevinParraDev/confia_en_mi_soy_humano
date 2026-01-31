using UnityEngine;

public class InteractableExample : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;

    private void Awake()
    {
        hoverGO.SetActive(false);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        hoverGO.SetActive(active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        Debug.Log("Do something");
    }
}
