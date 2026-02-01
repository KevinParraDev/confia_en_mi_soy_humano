using UnityEngine;

public class PanicState : IState
{
    private NPCBaseController npcController;
    private Transform panicRoomPosition;
    private bool hasPanicDestination = false;

    public PanicState(NPCBaseController npcController, Transform panicRoomPosition, bool hasPanicDestination)
    {
        this.npcController = npcController;
        this.panicRoomPosition = panicRoomPosition;
        this.hasPanicDestination = hasPanicDestination;
    }

    public void Enter()
    {
        if (hasPanicDestination)
        {
            npcController.SetNewDestination(panicRoomPosition.position);
        }
        Debug.Log("Enter Panic");
    }

    public void Execute()
    {
        if (hasPanicDestination)
        {
            if (npcController.HasReachedDestination())
            {
                npcController.StopMovement();
            }
            else
            {
                npcController.SetNewDestination(panicRoomPosition.position);
            }
        }
    }

    public void Exit() { }
}
