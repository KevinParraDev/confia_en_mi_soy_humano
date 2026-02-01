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
    public NpcDataSO GetData()
    {
        return npcData;
    }
    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            NPC playerSkin = playerController.GetCurrentSkin();

            dialogController.ShowDialog(view.GetCurrentSprite(), npcData.GetDialogForNpc(playerSkin));
        }
    }

    public void StartDialogue(NPC npcType)
    {
        dialogController.ShowDialog(view.GetCurrentSprite(), npcData.GetDialogForNpc(npcType));
    }
    public override void Transform(PlayerController playerController = null)
    {
        base.Transform();
        if (playerController != null)
        {
            if(npcData.npcType != NPC.Guard)
            {
                playerController.ChangeSkin(npcData);
                view.SetBoolAnimation(Constants.ANIM_SCARRY, true);

                if (this.TryGetComponent<NPCBaseController>(out NPCBaseController npcController))
                {
                    npcController.StunNPC();
                }
            }
        }
    }
}
