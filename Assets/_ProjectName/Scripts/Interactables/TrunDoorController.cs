using UnityEngine;

public class TrunDoorController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private GameObject doorGO;
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
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Trun)
            {
                Debug.Log("Abrir puerta");
                doorGO?.SetActive(false);
            }
            else
            {
                Debug.Log("Solo puedes llevar platos si eres mesero");
            }
        }
    }
}
