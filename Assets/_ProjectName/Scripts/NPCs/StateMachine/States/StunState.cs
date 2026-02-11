using UnityEngine;

public class StunState : IState
{
    NPCBaseController npcController;
    private float stunDuration;
    private float stunTimer;
    private bool permanentStun;

    public StunState(NPCBaseController npcController, float stunDuration, bool permanentStun)
    {
        this.npcController = npcController;
        this.stunDuration = stunDuration;
        this.permanentStun = permanentStun;
    }

    public void Enter()
    {
        stunTimer = 0f;
        npcController.StopMovement();
    }

    public void Execute()
    {
        if (permanentStun)
        {
            return;
        }

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
