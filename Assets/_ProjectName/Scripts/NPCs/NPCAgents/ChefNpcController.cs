using UnityEngine;

public class ChefNpcController : NPCBaseController
{
    [Header("Patrol Config")]
    [SerializeField] private Transform[] patrolWaypoints;
    [SerializeField] private float waitTimeAtWaypoint = 3f;

    [Header("Cooking Interaction Config")]
    [SerializeField] private InteractableBaseController cookingStation;
    [SerializeField] private float cookingDuration = 5f;
    [SerializeField] private float interactionDistance = 1f;

    protected override void SetupStateMachine()
    {
        InteractState cookState = new InteractState(this, cookingDuration);
        PatrolState patrolState = new PatrolState(this, patrolWaypoints, waitTimeAtWaypoint);

        stateMachine.AddState(NPCState.Interact, cookState);
        stateMachine.AddState(NPCState.Patrol, patrolState);

        stateMachine.ChangeState(NPCState.Patrol);
    }

    public override void Interact()
    {
        if(Vector3.Distance(transform.position, cookingStation.transform.position) < interactionDistance)
        {
            stateMachine.ChangeState(NPCState.Interact);
            this.StopMovement();
            Debug.Log("Chef NPC is interacting with the cooking station.");
        }
    }

    public override void StopInteract()
    {
        stateMachine.ChangeState(NPCState.Patrol);
        this.ResumeMovement();
        Debug.Log("Chef NPC has finished cooking and is resuming patrol.");
    }
}
