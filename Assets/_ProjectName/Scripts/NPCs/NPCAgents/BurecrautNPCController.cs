using UnityEngine;

public class BurecrautNPCController : NPCBaseController
{

    [Header("Idle Configuration")]
    [SerializeField] private Transform idlePosition;
    [SerializeField] private float idleCheckInterval = 5f;

    [Header("Panic Configuration")]
    [SerializeField] private Transform panicRoomPosition;
    [SerializeField] private bool goToPanicRoomOnAlarm = true;

    [Header("In Bath Configuration")]
    [SerializeField] private Transform bathPosition;
    [SerializeField] private float inBathDuration;

    protected override void SetupStateMachine()
    {
        IdleState idleState = new IdleState(this, idlePosition, idleCheckInterval, true);
        PanicState panicState = new PanicState(this, panicRoomPosition, goToPanicRoomOnAlarm);
        StunState stunState = new StunState(this, 3f);
        BathState bathState = new BathState(this, bathPosition, inBathDuration);
        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Panic, panicState);
        stateMachine.AddState(NPCState.Stun, stunState);
        stateMachine.AddState(NPCState.InBath, bathState);

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
                stateMachine.ChangeState(NPCState.Idle);
            }
        }
    }

    public override void Poison()
    {
        base.Poison();

        if(stateMachine.GetCurrentStateType() == NPCState.Idle)
        {
            stateMachine.ChangeState(NPCState.InBath);
        }
    }
}
