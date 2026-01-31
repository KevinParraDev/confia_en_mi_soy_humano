using UnityEngine;

public class GuardNPCController : NPCBaseController
{
    [Header("Chase Configuration")]
    [SerializeField]
    private float ChaseFovAngle = 90;
    [SerializeField]
    private float chaseFovDistance = 50;
    [SerializeField]
    private float chaseDuration;

    [SerializeField]
    private Transform IdlePlace;

    private bool playerInRange;

    

    protected override void SetupStateMachine()
    {
        IdleState idleState = new IdleState(this, IdlePlace, 0f, true);
        ChaseState chaseState = new ChaseState(this, detectionController, playerTransform, chaseDuration);

        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Chase, chaseState);

        stateMachine.ChangeState(NPCState.Idle);
    }

    private void FixedUpdate()
    {
        if(stateMachine.GetCurrentStateType() == NPCState.Idle)
        {
            if (detectionController.IsPlayerInRange)
            {
                stateMachine.ChangeState(NPCState.Chase);
                currentFovAngle = ChaseFovAngle;
                currentFovViewDistance = chaseFovDistance;
                npcFov.SetNewFOV(currentFovAngle, currentFovViewDistance);
            }
        }
    }

    public override void StopChase()
    {
        base.StopChase();
        this.StopMovement();
        stateMachine.ChangeState(NPCState.Idle);
        currentFovAngle = fovAngle;
        currentFovViewDistance = fovViewDistance;
        npcFov.SetNewFOV(currentFovAngle, currentFovViewDistance);
    }
}
