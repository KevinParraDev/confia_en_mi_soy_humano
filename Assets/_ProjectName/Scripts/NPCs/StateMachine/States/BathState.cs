using UnityEngine;

public class BathState : IState
{

    NPCBaseController npcController;
    private Transform bathroom;
    private float inBathDuration;
    private float timer;
    private bool stayInBath = true;

    private float timeToStartMoving = 3f;
    private float timerToMoving;
    private bool isMoving = false;

    public BathState(NPCBaseController npcController, Transform bathroom, float inBathDuration, bool stayInBath = true)
    {
        this.npcController = npcController;
        this.bathroom = bathroom;
        this.inBathDuration = inBathDuration;
        this.stayInBath = stayInBath;
    }

    public void Enter()
    {
        timerToMoving = 0;
        timer = 0;
        isMoving = false;
    }

    public void Execute()
    {

        if (timerToMoving < timeToStartMoving)
        {
            timerToMoving += Time.deltaTime;
            return;
        }

        if (!isMoving)
        {
            npcController.SetNewDestination(bathroom.position);
            npcController.ResumeMovement();
            isMoving = true;
        }

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
