using UnityEngine;

public class BureaucratNPCInteractable : NpcInteractableController
{
    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            //if(playerController.GetCurrentSkin() == NPC.Waiter)
            //{
            //    if(this.TryGetComponent<NPCBaseController>(out NPCBaseController npcController))
            //    {
            //        npcController.Poison();
            //        return;
            //    }
            //}

            base.Interact(playerController);
        }
    }
}
