using System;
using System.Collections;
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

    [Header("BackToIdle Configuration")]
    [SerializeField]
    private Transform IdlePlace;

    [Header("Collider Door")]
    [SerializeField]
    private GameObject doorCollider;

    [SerializeField]
    private NPC npcAllowed = NPC.Bureaucrat;

    private bool isInDialogue = false;
    private float dialogueDuration = 2f;
    private float dialogueTimer = 0f;

    public static Action onGuardChase;
    public static Action onGuardStopChase;

    protected override void SetupStateMachine()
    {
        BackToIdleState backToIdleState = new BackToIdleState(this, IdlePlace);
        IdleState idleState = new IdleState(this, 0f, true);
        ChaseState chaseState = new ChaseState(this, detectionController, playerTransform, chaseDuration);

        stateMachine.AddState(NPCState.Idle, idleState);
        stateMachine.AddState(NPCState.Chase, chaseState);
        stateMachine.AddState(NPCState.BackToIdle, backToIdleState);

        stateMachine.ChangeState(NPCState.Idle);
    }

    private void FixedUpdate()
    {
        if(stateMachine.GetCurrentStateType() != NPCState.Chase)
        {
            if (this.IsAlarmed)
            {
                if (detectionController.IsPlayerInRange)
                {
                    StartChase();
                }

                return;
            }
        }

        if (stateMachine.GetCurrentStateType() == NPCState.Idle)
        {
            if (this.IsAlarmed)
            {
                return;
            }

            if(isInDialogue)
            {
                dialogueTimer += Time.fixedDeltaTime;
                if(dialogueTimer >= dialogueDuration)
                {
                    isInDialogue = false;
                    dialogueTimer = 0f;
                } else
                {
                    return;
                }
            }
            
            else if (detectionController.IsPlayerInRange && this.HasReachedDestination())
            {
                // launch a dialogue & Trigger the menace level if not is the allowed type
                if (playerController.GetCurrentSkin() != npcAllowed)
                {
                    isInDialogue = true;
                    if(TryGetComponent(out NpcInteractableController npcInteractable))
                    {
                        npcInteractable.StartDialogue(playerController.GetCurrentSkin());
                    }
                    doorCollider.SetActive(true);
                    onSuspiciosAction?.Invoke(25, SuspicionType.WrongZone);
                }
                else
                {
                    doorCollider.SetActive(false);
                }
            }
            else if((stateMachine.GetCurrentStateType() == NPCState.Idle) && !doorCollider.activeSelf)
            {
                doorCollider.SetActive(true);
            }
        }

        if(stateMachine.GetCurrentStateType() == NPCState.Chase)
        {
            doorCollider.SetActive(false);
        }
    }

    public override void AlarmOnNPC()
    {
        base.AlarmOnNPC();
        StartChase();
    }

    private void StartChase()
    {
        stateMachine.ChangeState(NPCState.Chase);

        onGuardChase?.Invoke();

        currentFovAngle = ChaseFovAngle;
        currentFovViewDistance = chaseFovDistance;
        npcFov.SetNewFOV(currentFovAngle, currentFovViewDistance);
    }

    public override void StopChase()
    {
        base.StopChase();
        currentFovAngle = fovAngle;
        currentFovViewDistance = fovViewDistance;
        npcFov.SetNewFOV(currentFovAngle, currentFovViewDistance);
        this.StopMovement();
        

        stateMachine.ChangeState(NPCState.BackToIdle);

        onGuardStopChase?.Invoke();
    }
}
