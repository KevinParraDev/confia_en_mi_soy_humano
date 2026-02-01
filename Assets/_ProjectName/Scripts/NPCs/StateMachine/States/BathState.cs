using UnityEngine;

public class BathState : IState
{

    NPCBaseController npcController;
    private Transform bathroom;
    private float inBathDuration;
    private float timer;

    public BathState(NPCBaseController npcController, Transform bathroom, float inBathDuration)
    {
        this.npcController = npcController;
        this.bathroom = bathroom;
        this.inBathDuration = inBathDuration;
    }

    public void Enter()
    {
        npcController.SetNewDestination(bathroom.position);
    }

    public void Execute()
    {
        if (npcController.HasReachedDestination())
        {
            timer += Time.deltaTime;

            if (timer > inBathDuration)
            {
                timer = 0;
                npcController.BackToIdle();
            }
        }
    }

    public void Exit()
    {
    }
}
