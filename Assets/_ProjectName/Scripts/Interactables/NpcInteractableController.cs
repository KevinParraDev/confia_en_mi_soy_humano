using UnityEngine;

public class NpcInteractableController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private Animator skin;

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
        if(playerController != null)
        {
            Debug.Log("playerController: " + playerController);
            Debug.Log("skin: " + skin);
            playerController.ChangeSkin(skin.runtimeAnimatorController);
        }
    }
}
