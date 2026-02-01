using UnityEngine;

public class BurecrautNPCController : NPCBaseController
{

    [Header("Idle Configuration")]
    [SerializeField] private Transform idlePosition;
    [SerializeField] private float idleCheckInterval = 5f;

    [Header("Panic Configuration")]
    [SerializeField] private Transform panicRoomPosition;
    [SerializeField] private bool goToPanicRoomOnAlarm = true;

    protected override void SetupStateMachine()
    {
        IdleState idleState = new IdleState(this, idlePosition, idleCheckInterval, true);
        PanicState panicState = new PanicState(this, panicRoomPosition, goToPanicRoomOnAlarm);
        StunState stunState = new StunState(this, 3f);

        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Panic, panicState);
        stateMachine.AddState(NPCState.Stun, stunState);

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
}
