using UnityEngine;

public class BackToIdleState : IState
{

    private NPCBaseController npcController;
    private Transform idlePosition;

    public BackToIdleState(NPCBaseController npcController, Transform idlePosition)
    {
        this.npcController = npcController;
        this.idlePosition = idlePosition;
    }

    public void Enter()
    {
        npcController.SetNewDestination(idlePosition.position);

        npcController.ResumeMovement();
    }

    public void Execute()
    {
        if (npcController.HasReachedDestination())
        {
            npcController.BackToIdle();

            return;
        }

        npcController.SetNewDestination(idlePosition.position);
    }

    public void Exit()
    {
    }
}
