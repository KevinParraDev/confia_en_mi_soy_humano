using UnityEngine;

public class BathState : IState
{

    NPCBaseController npcController;
    private Transform bathroom;
    private float inBathDuration;
    private float timer;
    private bool stayInBath = true;

    public BathState(NPCBaseController npcController, Transform bathroom, float inBathDuration, bool stayInBath = true)
    {
        this.npcController = npcController;
        this.bathroom = bathroom;
        this.inBathDuration = inBathDuration;
        this.stayInBath = stayInBath;
    }

    public void Enter()
    {
        npcController.SetNewDestination(bathroom.position);
        npcController.ResumeMovement();
    }

    public void Execute()
    {
        

        if (npcController.HasReachedDestination())
        {
            if (stayInBath)
            {
                npcController.BackToIdle();
                return;
            }
                
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
