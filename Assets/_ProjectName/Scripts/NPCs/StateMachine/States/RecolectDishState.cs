using UnityEngine;

public class RecolectDishState : IState
{
    private WaiterNPCController npcController;
    private float recolectionDuration = 5f;
    private float recolectionTimer = 0f;
    private DishesDeskInteractable serviceStation;
    private Transform recolectionPoint;

    public RecolectDishState(WaiterNPCController controller, float duration, Transform recolectionPoint, DishesDeskInteractable serveStation)
    {
        npcController = controller;
        recolectionDuration = duration;
        this.serviceStation = serveStation;
        this.recolectionPoint = recolectionPoint;
    }

    public void Enter()
    {
        recolectionTimer = 0f;
        npcController.StopMovement();
        npcController.SetNewDestination(recolectionPoint.position);
        npcController.ResumeMovement();
    }

    public void Execute()
    {
        if (npcController.HasReachedDestination())
        {
            recolectionTimer += Time.deltaTime;
            if (recolectionTimer >= recolectionDuration)
            {
                serviceStation.RemoveDish(1);
                npcController.RecolectDish();
                npcController.StopMovement();
                npcController.BackToPatrol();
            }
            return;
        }

        npcController.SetNewDestination(recolectionPoint.position);
        npcController.ResumeMovement();
    }

    public void Exit()
    {
        // Any cleanup if necessary
    }
}
