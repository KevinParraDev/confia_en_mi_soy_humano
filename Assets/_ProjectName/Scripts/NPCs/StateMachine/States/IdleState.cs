using UnityEngine;

public class IdleState : IState
{
    private NPCBaseController npcController;
    private Transform idlePosition;
    private float idleDuration = 2f;
    private float idleTimer = 0f;
    private bool isAlwaysIdle = false;

    public IdleState(NPCBaseController npcController, Transform idlePosition, float idleTimer, bool isAlwaysIdle)
    {
        this.npcController = npcController;
        this.idlePosition = idlePosition;
        this.idleTimer = idleTimer;
        this.isAlwaysIdle = isAlwaysIdle;
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
            npcController.StopMovement();
        }
        else
        {
            npcController.SetNewDestination(idlePosition.position);
        }

        if (npcController.HasReachedDestination() && !isAlwaysIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                npcController.StopIdleCheck();
                idleTimer = 0f;
            }
        }
    }

    public void Exit()
    {
    }
}
