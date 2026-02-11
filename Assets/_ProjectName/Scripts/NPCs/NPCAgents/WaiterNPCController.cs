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
    private bool isWaitingForDishes = false;

    [Header("Back To Idle Configuration")]
    [SerializeField] private Transform idlePosition;

    [Header("Idle Configuration")]
    [SerializeField] private float idleCheckInterval = 5f;

    [Header("Panic Configuration")]
    [SerializeField] private Transform panicRoomPosition;
    [SerializeField] private bool goToPanicRoomOnAlarm = false;

    [Header("InitialDialogue State")]
    [SerializeField] private Transform initialDialoguePosition;
    [TextArea(3, 5)]
    [SerializeField] private string dialogueText;

    protected override void SetupStateMachine()
    {


        InteractState serveState = new InteractState(this, servingDuration);
        PatrolState patrolState = new PatrolState(this, patrolPoints, waitTimeAtWaypoint);
        BackToIdleState backToIdleState = new BackToIdleState(this, idlePosition);
        IdleState idleState = new IdleState(this, idleCheckInterval, false);
        StunState stunState = new StunState(this, 3f, true);
        PanicState panicState = new PanicState(this, panicRoomPosition, goToPanicRoomOnAlarm);
        InitialDialogueState initialDialogue = new InitialDialogueState(this, initialDialoguePosition);

        stateMachine.AddState(NPCState.Interact, serveState);
        stateMachine.AddState(NPCState.Patrol, patrolState);
        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Stun, stunState);
        stateMachine.AddState(NPCState.Panic, panicState);
        stateMachine.AddState(NPCState.BackToIdle, backToIdleState);
        stateMachine.AddState(NPCState.GoToDialogue, initialDialogue);

        stateMachine.ChangeState(NPCState.Idle);
    }

    public override void Interact()
    {
        if (Vector3.Distance(transform.position, servingStation.transform.position) < interactionDistance)
        {
            stateMachine.ChangeState(NPCState.Interact);
        }
    }

    public override void StopInteract()
    {
        if (servingStation.GetDishCount() > patrolPoints.Length - 1)
        {
            servingStation.RemoveDish(1);
            stateMachine.ChangeState(NPCState.Patrol);
        }
        else
        {
            stateMachine.ChangeState(NPCState.GoToDialogue);
        }
        this.ResumeMovement();
    }

    public override void StopIdleCheck()
    {
        if (!isWaitingForDishes)
            return;

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
            SetToInitialDialogue();
        }
    }

    private void FixedUpdate()
    {
        if (stateMachine.GetCurrentStateType() != NPCState.Panic)
        {
            if (this.IsAlarmed)
            {
                stateMachine.ChangeState(NPCState.Panic);
            }
        }

        if (stateMachine.GetCurrentStateType() == NPCState.Panic)
        {
            if (!this.IsAlarmed)
            {
                stateMachine.ChangeState(NPCState.Idle);
            }
        }
    }

    public override void SetToInitialDialogue()
    {
        stateMachine.ChangeState(NPCState.GoToDialogue);
    }

    public override void ShowInitialDialogue()
    {
        if (TryGetComponent(out NpcInteractableController npcInteractable))
        {
            npcInteractable.StartDialogue(dialogueText);
        }
    }

    public override void CloseInitialDialogue()
    {
        if (isWaitingForDishes)
        {
            onSuspiciosAction?.Invoke(5, SuspicionType.FoodLack);
        }

        if (TryGetComponent(out NpcInteractableController npcInteractable))
        {
            npcInteractable.CloseDialogue();
        }

        stateMachine.ChangeState(NPCState.BackToIdle);

        isWaitingForDishes = true;
    }
}
