using UnityEngine;

public class NpcInteractableController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private NpcDataSO npcData;
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
        Debug.Log("Try talk");
    }
    public override void Transform(PlayerController playerController = null)
    {
        base.Transform();
        if (playerController != null)
        {
            playerController.ChangeSkin(npcData);
        }
    }
}
