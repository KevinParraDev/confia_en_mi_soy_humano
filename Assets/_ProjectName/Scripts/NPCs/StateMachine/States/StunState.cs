using UnityEngine;

public class StunState : IState
{
    NPCBaseController npcController;
    private float stunDuration;
    private float stunTimer;

    public StunState(NPCBaseController npcController, float stunDuration)
    {
        this.npcController = npcController;
        this.stunDuration = stunDuration;
    }

    public void Enter()
    {
        stunTimer = 0f;
        npcController.StopMovement();
        Debug.Log("NPC Stunned");
    }

    public void Execute()
    {
        stunTimer += Time.deltaTime;
        if (stunTimer >= stunDuration)
        {
            npcController.BackToIdle();
        }
    }

    public void Exit()
    {
    }

}
