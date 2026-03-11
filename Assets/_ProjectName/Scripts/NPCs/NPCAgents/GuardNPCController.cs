using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private KevinTestState kevinTestState;

    [Header("NPC FOV Config")]
    [SerializeField] protected NPCFOVController npcFov;
    [SerializeField] protected float fovAngle = 90f;
    [SerializeField] protected float fovViewDistance = 3f;
    protected NPCDetectionController detectionController;

    protected float currentFovAngle;
    protected float currentFovViewDistance;

    [Header("BackToIdle Configuration")]
    [SerializeField]
    private Transform IdlePlace;

    [Header("Collider Door")]
    [SerializeField]
    private GameObject doorCollider;

    [SerializeField]
    private NPC npcAllowed = NPC.Bureaucrat_1;

    private bool isInDialogue = false;
    private float dialogueDuration = 2f;
    private float dialogueTimer = 0f;

    public static Action onGuardChase;
    public static Action onGuardStopChase;

    private void InitiaizeFOV()
    {
        detectionController = GetComponent<NPCDetectionController>();

        if (detectionController == null)
        {
            Debug.LogWarning("NPCDetectionController Component Not Set");
        }

        currentFovAngle = fovAngle;
        currentFovViewDistance = fovViewDistance;
        npcFov.Initialize(currentFovAngle, currentFovViewDistance);
        detectionController.Initialize(playerTransform.transform, this.transform);
    }

    protected override void SetupStateMachine()
    {
        InitiaizeFOV();

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
        if (stateMachine.GetCurrentStateType() != NPCState.Chase)
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
                Debug.Log("Permitido: " + npcAllowed + " - Player: " + playerController.GetCurrentSkin());
                if (npcAllowed != playerController.GetCurrentSkin())
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
            if (agent.remainingDistance <= 0.5f && !agent.pathPending)
            {
                Debug.Log("Guard has reached the player during chase.");
                kevinTestState.GameOver();
            }
        }
    }

    private void Update()
    {
        npcFov.SetAimDirection(GetAimDirection());
        npcFov.SetOrigin(transform.position);
        detectionController.FindPlayer(GetAimDirection(), currentFovViewDistance, currentFovAngle);
    }

    private Vector3 GetAimDirection()
    {
        Vector3 direction = agent.velocity.normalized;
        characterView.Turn(direction.x);
        if (direction == Vector3.zero)
        {
            direction = transform.up;
        }
        return direction;
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
