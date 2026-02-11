using UnityEngine;

public class BurecrautNPCController : NPCBaseController
{

    [Header("Back To Idle Configuration")]
    [SerializeField] private Transform idlePosition;

    [Header("Idle Configuration")]
    [SerializeField] private float idleCheckInterval = 5f;

    [Header("Panic Configuration")]
    [SerializeField] private Transform panicRoomPosition;
    [SerializeField] private bool goToPanicRoomOnAlarm = true;

    [Header("In Bath Configuration")]
    [SerializeField] private Transform bathPosition;
    [SerializeField] private float inBathDuration;

    protected override void SetupStateMachine()
    {
        BackToIdleState backToIdleState = new BackToIdleState(this, idlePosition);
        IdleState idleState = new IdleState(this, idleCheckInterval, true);
        PanicState panicState = new PanicState(this, panicRoomPosition, goToPanicRoomOnAlarm);
        StunState stunState = new StunState(this, 3f, true);
        BathState bathState = new BathState(this, bathPosition, inBathDuration);
        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Panic, panicState);
        stateMachine.AddState(NPCState.Stun, stunState);
        stateMachine.AddState(NPCState.InBath, bathState);
        stateMachine.AddState(NPCState.BackToIdle, backToIdleState);

        stateMachine.ChangeState(NPCState.Idle);
    }

    private void FixedUpdate()
    {
        if (stateMachine.GetCurrentStateType() == NPCState.Idle)
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
                stateMachine.ChangeState(NPCState.BackToIdle);
            }
        }
    }

    public override void Poison()
    {
        base.Poison();

        if(stateMachine.GetCurrentStateType() == NPCState.Idle)
        {
            Debug.Log("Bureaucrat NPC poisoned, going to bath.");
            stateMachine.ChangeState(NPCState.InBath);
        }
    }
}
