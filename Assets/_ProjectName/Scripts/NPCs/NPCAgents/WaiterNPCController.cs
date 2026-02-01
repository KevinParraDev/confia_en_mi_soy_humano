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

    [Header("Panic Configuration")]
    [SerializeField] private Transform panicRoomPosition;
    [SerializeField] private bool goToPanicRoomOnAlarm = false;

    protected override void SetupStateMachine()
    {


        InteractState serveState = new InteractState(this, servingDuration);
        PatrolState patrolState = new PatrolState(this, patrolPoints, waitTimeAtWaypoint);
        IdleState idleState = new IdleState(this, idlePosition, idleCheckInterval, false);
        StunState stunState = new StunState(this, 3f);
        PanicState panicState = new PanicState(this, panicRoomPosition, goToPanicRoomOnAlarm);
        stateMachine.AddState(NPCState.Interact, serveState);
        stateMachine.AddState(NPCState.Patrol, patrolState);
        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Stun, stunState);
        stateMachine.AddState(NPCState.Panic, panicState);
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
            onSuspiciosAction?.Invoke(5, SuspicionType.FoodLack);
        }
    }

    private void FixedUpdate()
    {
        if (stateMachine.GetCurrentStateType() == NPCState.Idle)
        {
            if (this.IsAlarmed)
            {
                Debug.Log("In Panic");
                stateMachine.ChangeState(NPCState.Panic);
            }
        }

        if (stateMachine.GetCurrentStateType() == NPCState.Panic)
        {
            if (!this.IsAlarmed)
            {
                Debug.Log("Out Of Panic");
                stateMachine.ChangeState(NPCState.Idle);
            }
        }
    }
}
