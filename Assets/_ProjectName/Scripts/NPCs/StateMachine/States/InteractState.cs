using UnityEngine;

public class InteractState : IState
{
    private NPCBaseController npcController;
    private float interactionDuration = 5f;
    private float interactionTimer = 0f;

    public InteractState(NPCBaseController controller, float duration)
    {
        npcController = controller;
        interactionDuration = duration;
    }

    public void Enter()
    {
        interactionTimer = 0f;
    }

    public void Execute()
    {
        interactionTimer += Time.deltaTime;
        if (interactionTimer >= interactionDuration)
        {
            npcController.StopInteract();
        }
    }

    public void Exit()
    {
        // Any cleanup if necessary
    }
}
