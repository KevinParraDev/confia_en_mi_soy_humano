using UnityEngine;

public class ChaseState : IState
{
    private NPCBaseController npcController;
    private NPCDetectionController npcDetectionController;
    private Transform player;
    private float chaseDuration;
    private float chaseTime;

    public ChaseState(NPCBaseController npcController, NPCDetectionController npcDetectionController, Transform player, float chaseDuration)
    {
        this.npcController = npcController;
        this.npcDetectionController = npcDetectionController;
        this.player = player;
        this.chaseDuration = chaseDuration;
    }

    public void Enter()
    {
        this.npcController.SetNewDestination(player.position);
        this.npcController.ResumeMovement();
        chaseTime = 0;
    }

    public void Execute()
    {
        if (this.player != null)
        {
            if (!npcDetectionController.IsPlayerInRange)
            {
                chaseTime += Time.deltaTime;
                if (chaseTime > chaseDuration)
                {
                    npcController.StopChase();
                }
            }

            else
            {
                chaseTime = 0;
            }
        }

        npcController.SetNewDestination(player.position);
    }

    public void Exit()
    {
    }
}
