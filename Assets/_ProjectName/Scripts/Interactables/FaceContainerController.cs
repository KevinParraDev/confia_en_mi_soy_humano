using System;
using UnityEngine;

public class FaceContainerController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private Animator panelInfoAnim;
    [SerializeField] private NpcDataSO npcData;
    private bool isScared = false;

    public static Action onNPCIsCopied;

    protected override void Awake()
    {
        base.Awake();
        hoverGO.SetActive(false);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        view.Hover(active);
        hoverGO.SetActive(active);
        panelInfoAnim.SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }
    public NpcDataSO GetData()
    {
        return npcData;
    }
    public override void Interact(PlayerController playerController = null)
    {
        if (playerController == null) return;

        NPC playerSkin = playerController.GetCurrentSkin();

        DialogNode startNode = npcData.GetStartNode(playerSkin, isScared);
    }


    [ContextMenu("Transform")]
    public override void Transform(PlayerController playerController = null)
    {
        base.Transform();
        if (playerController != null)
        {
            Debug.Log("NPC Type: " + npcData.npcType + " Player Skin: " + playerController.GetCurrentSkin());
            if (playerController.GetCurrentSkin() == npcData.npcType)
            {
                return;
            }

            playerController.ChangeSkin(npcData);
            onNPCIsCopied?.Invoke();
        }
    }
}
