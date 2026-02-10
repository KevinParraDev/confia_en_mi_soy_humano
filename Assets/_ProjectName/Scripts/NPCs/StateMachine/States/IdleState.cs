using UnityEngine;

public class IdleState : IState
{
    private NPCBaseController npcController;
    private float idleDuration = 2f;
    private float idleTimer = 0f;
    private bool isAlwaysIdle = false;

    public IdleState(NPCBaseController npcController, float idleTimer, bool isAlwaysIdle)
    {
        this.npcController = npcController;
        this.idleTimer = idleTimer;
        this.isAlwaysIdle = isAlwaysIdle;
    }

    public void Enter()
    {
        npcController.StopMovement();
    }

    public void Execute()
    {
        if (!isAlwaysIdle)
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
