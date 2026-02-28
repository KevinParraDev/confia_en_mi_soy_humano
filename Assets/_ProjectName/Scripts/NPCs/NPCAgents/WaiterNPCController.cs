using System.Linq;
using UnityEngine;

public class WaiterNPCController : NPCBaseController
{
    [Header("Patrol Configuration")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtWaypoint = 3f;

    [Header("Serving Interaction Configuration")]
    [SerializeField] private DishesDeskInteractable servingStation;
    [SerializeField] private Transform servingPoint;
    [SerializeField] private float servingDuration = 1f;
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
    [TextArea(2, 3)]
    [SerializeField] private string startDialogue;
    [TextArea(2, 3)]
    [SerializeField] private string waitingDialogue;

    [Header("Service Finished")]
    [TextArea(2, 3)]
    [SerializeField] private string serviceFinishedDialogue;
    [SerializeField] private int dishesToRecolect = 3;
    [SerializeField] private GameObject kitchenDoor;

    protected override void SetupStateMachine()
    {
        RecolectDishState serveState = new RecolectDishState(this, servingDuration, servingPoint, servingStation);
        PatrolState patrolState = new PatrolState(this, patrolPoints, waitTimeAtWaypoint);
        BackToIdleState backToIdleState = new BackToIdleState(this, idlePosition);
        IdleState idleState = new IdleState(this, idleCheckInterval, false);
        StunState stunState = new StunState(this, 3f, true);
        PanicState panicState = new PanicState(this, panicRoomPosition, goToPanicRoomOnAlarm);
        DialogueState initialDialogue = new DialogueState(this, initialDialoguePosition);

        stateMachine.AddState(NPCState.Recolect, serveState);
        stateMachine.AddState(NPCState.Patrol, patrolState);
        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Stun, stunState);
        stateMachine.AddState(NPCState.Panic, panicState);
        stateMachine.AddState(NPCState.BackToIdle, backToIdleState);
        stateMachine.AddState(NPCState.GoToDialogue, initialDialogue);

        stateMachine.ChangeState(NPCState.Idle);
    }

    public override void StopIdleCheck()
    {
        if (!isWaitingForDishes)
            return;

        if (dishesToRecolect > 0)
        {
            if (servingStation.GetDishCount() >= 1)
            {
                stateMachine.ChangeState(NPCState.Recolect);
                this.ResumeMovement();
                return;
            }
        }

        SetToDialogue();
    }

    private void FixedUpdate()
    {
        if (stateMachine.GetCurrentStateType() != NPCState.Panic)
        {
            if (this.IsAlarmed)
            {
                stateMachine.ChangeState(NPCState.Panic);
                isWaitingForDishes = false;
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

    public override void SetToDialogue()
    {
        stateMachine.ChangeState(NPCState.GoToDialogue);
    }

    public override void ShowDialogue()
    {
        if (TryGetComponent(out NpcInteractableController npcInteractable))
        {
            if(dishesToRecolect <= 0)
            {
                npcInteractable.StartDialogue(serviceFinishedDialogue);
                isWaitingForDishes = false;
            } else if(dishesToRecolect >= 0 && isWaitingForDishes)
            {
                npcInteractable.StartDialogue(waitingDialogue);
            }
            else
            {
                npcInteractable.StartDialogue(startDialogue);

                // Update Task Progress
                TaskListManager.Instance?.OnUpdateNextTask();
            }

            if (playerController.TryGetComponent(out PlayerMovementController playerMovement))
            {
                playerMovement.StopMovement();
            }
        }
    }

    public override void CloseDialogue()
    {
        if (isWaitingForDishes)
        {
            onSuspiciosAction?.Invoke(15, SuspicionType.FoodLack);
        }

        if (TryGetComponent(out NpcInteractableController npcInteractable))
        {
            npcInteractable.CloseDialogue();

            if (playerController.TryGetComponent(out PlayerMovementController playerMovement))
            {
                playerMovement.ResumeMovement();
            }

        }

        if(dishesToRecolect <= 0)
        {
            stateMachine.ChangeState(NPCState.Idle);
            kitchenDoor.SetActive(false);
            isWaitingForDishes = false;

            // Update Task To Next Task
            TaskListManager.Instance?.OnUpdateNextTask();
        }
        else
        {
            stateMachine.ChangeState(NPCState.BackToIdle);

            isWaitingForDishes = true;
        } 
    }

    public override void StunNPC()
    {
        base.StunNPC();
        kitchenDoor.SetActive(false);
    }

    public void RecolectDish()
    {
        dishesToRecolect -= 1;

        // Update Task Progress
        TaskListManager.Instance?.OnTaskEvent(TaskType.CocinarPlatos, 1);
    }
}
