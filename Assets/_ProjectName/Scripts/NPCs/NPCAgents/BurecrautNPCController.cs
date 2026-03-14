using UnityEngine;

public class BurecrautNPCController : NPCBaseController
{

    [Header("Back To Idle Configuration")]
    [SerializeField] private Transform idlePosition;

    [Header("Panic Configuration")]
    [SerializeField] private Transform panicRoomPosition;
    [SerializeField] private bool goToPanicRoomOnAlarm = true;

    [Header("In Bath Configuration")]
    [TextArea(2, 3)]
    [SerializeField] private string poisonDialogue;
    [SerializeField] private Transform bathPosition;
    [SerializeField] private float inBathDuration;

    protected override void SetupStateMachine()
    {
        BackToIdleState backToIdleState = new BackToIdleState(this, idlePosition);
        IdleState idleState = new IdleState(this, 5f, true);
        PanicState panicState = new PanicState(this, panicRoomPosition, goToPanicRoomOnAlarm);
        StunState stunState = new StunState(this, 3f, true);
        BathState bathState = new BathState(this, bathPosition, inBathDuration);
        DialogueState dialogueState = new DialogueState(this, idlePosition);

        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Panic, panicState);
        stateMachine.AddState(NPCState.Stun, stunState);
        stateMachine.AddState(NPCState.InBath, bathState);
        stateMachine.AddState(NPCState.BackToIdle, backToIdleState);
        stateMachine.AddState(NPCState.GoToDialogue, dialogueState);

        stateMachine.ChangeState(NPCState.Idle);
    }

    private void FixedUpdate()
    {
        //if (stateMachine.GetCurrentStateType() == NPCState.Idle)
        //{
        //    if (this.IsAlarmed)
        //    {
        //        stateMachine.ChangeState(NPCState.Panic);
        //    }
        //}

        //if (stateMachine.GetCurrentStateType() == NPCState.Panic)
        //{
        //    if (!this.IsAlarmed)
        //    {
        //        stateMachine.ChangeState(NPCState.Idle);
        //    }
        //}
    }

    public override void Poison()
    {
        base.Poison();

        if(stateMachine.GetCurrentStateType() == NPCState.Idle)
        {
            //stateMachine.ChangeState(NPCState.GoToDialogue);
            stateMachine.ChangeState(NPCState.InBath);
        }
    }

    public override void ShowDialogue()
    {
        if (TryGetComponent(out NpcInteractableController npcInteractable))
        {
            npcInteractable.StartDialogue(poisonDialogue);
        }
    }

    public override void CloseDialogue()
    {

        if (TryGetComponent(out NpcInteractableController npcInteractable))
        {
            npcInteractable.CloseDialogue();
        }

        stateMachine.ChangeState(NPCState.InBath);
    }
}
