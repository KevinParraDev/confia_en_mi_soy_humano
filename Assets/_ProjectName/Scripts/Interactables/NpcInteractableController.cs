using UnityEngine;

public class NpcInteractableController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private NpcDataSO npcData;
    [SerializeField] private NpcDialogController dialogController;
    private CharacterView view;
    private void Awake()
    {
        view = GetComponentInChildren<CharacterView>();
        hoverGO.SetActive(false);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        hoverGO.SetActive(active);

        if(!active)
        {
            dialogController.Close();
        }
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            NPC playerSkin = playerController.GetCurrentSkin();

            dialogController.ShowDialog(view.GetCurrentSprite(), npcData.GetDialogForNpc(playerSkin));
        }
    }
    public override void Transform(PlayerController playerController = null)
    {
        base.Transform();
        if (playerController != null)
        {
            playerController.ChangeSkin(npcData);

            if(this.TryGetComponent<NPCBaseController>(out NPCBaseController npcController))
            {
                npcController.StunNPC();
            }
        }
    }
}
