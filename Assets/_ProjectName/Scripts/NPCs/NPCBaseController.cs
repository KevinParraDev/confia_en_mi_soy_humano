using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NPCBaseController : MonoBehaviour
{
    [Header("NPC Configuration")]
    [SerializeField] protected float moveSpeed = 3.5f;

    protected NavMeshAgent agent;
    protected StateMachine stateMachine;

    // Provisional Method
    protected Transform playerTransform;
    protected PlayerController playerController;
    protected CharacterView characterView;

    private bool isAlarmed;
    public bool IsAlarmed { get { return isAlarmed; } }

    // Do Alarm Increase Event
    public static Action<int, SuspicionType> onSuspiciosAction; // Bar Increase Amount, SuspicionAction

    private void Awake()
    {
        Initialize(FindAnyObjectByType<PlayerController>());
    }
    public void Initialize(PlayerController _player)
    {
        characterView = GetComponentInChildren<CharacterView>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();

        if (agent == null)
        {
            Debug.LogWarning("Agent Component Not set");
        }
        if (stateMachine == null)
        {
            Debug.LogWarning("Statem Machine Component Not Set");
        }



        // TODO : Pass Player Transform Dynamically

        playerTransform = _player.transform;
        playerController = _player;
        SetupStateMachine();

        // Initialize
        stateMachine.Initialize();


        // Alarm Config
        AlarmOffNPC();
        AlarmBarController.onPanicAlarm += AlarmOnNPC;
        AlarmBarController.onAlarmStopped += AlarmOffNPC;

        // TODO: Replace for detection in room
        // NpcInteractableController.onNPCIsCopied += AnyNPCIsCopied;

        // Agent Config
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    public void Conclude()
    {
        AlarmBarController.onPanicAlarm -= AlarmOnNPC;
        AlarmBarController.onAlarmStopped -= AlarmOffNPC;

        // TODO: Replace for detection in room
        // NpcInteractableController.onNPCIsCopied -= AnyNPCIsCopied;
    }

    private void Update()
    {
        Vector3 direction = agent.velocity.normalized;
        characterView.Turn(direction.x);
    }

    protected abstract void SetupStateMachine();

    public virtual void AlarmOnNPC()
    {
        isAlarmed = true;
    }

    public virtual void AlarmOffNPC()
    {
        isAlarmed = false;
    }

    public void SetNewDestination(Vector3 position)
    {
        agent.destination = position;
        ResumeMovement();
    }

    public void StopMovement()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        characterView.SetBoolAnimation(Constants.ANIM_MOVING, false);
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
        characterView.SetBoolAnimation(Constants.ANIM_MOVING, true);
    }

    public bool HasReachedDestination(float thresholdDistance = 0.01f)
    {
        if (!agent.pathPending && agent.remainingDistance < thresholdDistance)
        {
            return true;
        }

        return false;
    }

    public virtual void BackToIdle()
    {
        stateMachine.ChangeState(NPCState.Idle);
    }

    public virtual void StunNPC()
    {
        stateMachine.ChangeState(NPCState.Stun);
    }

    public virtual void SetToInitialDialogue() { }

    public virtual void ShowInitialDialogue() { }

    public virtual void CloseInitialDialogue() { }

    public virtual void Interact() { }

    public virtual void StopInteract() { }

    public virtual void StopIdleCheck() { }

    public virtual void StopChase() { }

    public virtual void Poison() { }
}
