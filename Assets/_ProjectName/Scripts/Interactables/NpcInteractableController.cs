using System;
using UnityEngine;

public class NpcInteractableController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private NpcDataSO npcData;
    [SerializeField] private NpcDialogController dialogController;
    private CharacterView view;

    public static Action onNPCIsCopied;

    private void Awake()
    {
        view = GetComponentInChildren<CharacterView>();
        hoverGO.SetActive(false);

        if(npcData.npcType == NPC.Chef)
            dialogController.OnConversationEnded += Panic;
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        //hoverGO.SetActive(active);
        view.Hover(active);

        if (!active)
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
        if (playerController == null) return;

        NPC playerSkin = playerController.GetCurrentSkin();

        DialogNode startNode = npcData.GetStartNode(playerSkin);

        if (startNode != null)
        {
            dialogController.StartConversation(view.GetCurrentSprite(), startNode, npcData);
        }
    }
    public void Panic()
    {
        view.SetBoolAnimation(Constants.ANIM_SCARRY, true);
    }
    public void StartDialogue(NPC npcType)
    {
        DialogNode startNode = npcData.GetStartNode(npcType);

        if (startNode != null)
        {
            dialogController.StartConversation(view.GetCurrentSprite(), startNode, npcData);
        }

        //dialogController.ShowDialog(view.GetCurrentSprite(), npcData.GetDialogForNpc(npcType));
    }

    public void StartDialogue(string dialogText)
    {
        dialogController.ShowDialog(view.GetCurrentSprite(), dialogText);
    }

    public void CloseDialogue()
    {
        dialogController.Close();
    }

    [ContextMenu("Transform")]
    public override void Transform(PlayerController playerController = null)
    {
        base.Transform();
        if (playerController != null)
        {
            if(npcData.npcType != NPC.Guard)
            {
                playerController.ChangeSkin(npcData);
                onNPCIsCopied?.Invoke();
                view.SetBoolAnimation(Constants.ANIM_SCARRY, true);
                npcData.isScared = true;

                if (this.TryGetComponent<NPCBaseController>(out NPCBaseController npcController))
                {
                    npcController.StunNPC();
                }
            }
        }
    }
}
