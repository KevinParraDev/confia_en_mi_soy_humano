using System.Linq;
using UnityEngine;

public class WaiterNPCController : NPCBaseController
{
    [Header("Patrol Configuration")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtWaypoint = 3f;

    [Header("Serving Interaction Configuration")]
    [SerializeField] private DishesDeskInteractable servingStation;
    [SerializeField] private float servingDuration = 1f;
    [SerializeField] private float interactionDistance = 1f;

    [Header("Idle Configuration")]
    [SerializeField] private Transform idlePosition;
    [SerializeField] private float idleCheckInterval = 5f;

    protected override void SetupStateMachine()
    {


        InteractState serveState = new InteractState(this, servingDuration);
        PatrolState patrolState = new PatrolState(this, patrolPoints, waitTimeAtWaypoint);
        IdleState idleState = new IdleState(this, idlePosition, idleCheckInterval, false);
        stateMachine.AddState(NPCState.Interact, serveState);
        stateMachine.AddState(NPCState.Patrol, patrolState);
        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.ChangeState(NPCState.Idle);
    }

    public override void Interact()
    {
        if (Vector3.Distance(transform.position, servingStation.transform.position) < interactionDistance)
        {
            stateMachine.ChangeState(NPCState.Interact);
            this.StopMovement();
        }

        if (stateMachine.GetCurrentState() is PatrolState patrolState)
        {
            if(patrolState.GetCurrentWaypointIndex() == patrolPoints.Length - 1)
            {
                // TODO: Disapear Dishes in the plate

                if (servingStation.GetDishCount() < patrolPoints.Length - 1)
                {
                    stateMachine.ChangeState(NPCState.Idle);
                }
            }
        }
    }

    public override void StopInteract()
    {
        if (servingStation.GetDishCount() > patrolPoints.Length - 1)
        {
            servingStation.RemoveDish(patrolPoints.Length - 1);
            stateMachine.ChangeState(NPCState.Patrol);
        }
        else
        {
            stateMachine.ChangeState(NPCState.Idle);
        }
        this.ResumeMovement();
    }

    public override void StopIdleCheck()
    {
        if (servingStation.GetDishCount() >= patrolPoints.Length - 1)
        {
            stateMachine.ChangeState(NPCState.Patrol);
            if(stateMachine.GetCurrentState() is PatrolState patrolState)
            {
                patrolState.ResetPatrol();
            }
            this.ResumeMovement();
        }
        else
        {
            onSuspiciosAction.Invoke(40, SuspicionType.FoodLack);
        }
    }
}
