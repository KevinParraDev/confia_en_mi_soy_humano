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
    protected NPCBaseView npcView;

    [Header("NPC FOV Config")]
    [SerializeField] protected NPCFOVController npcFov;
    [SerializeField] protected float fovAngle = 90f;
    [SerializeField] protected float fovViewDistance = 3f;
    protected NPCDetectionController detectionController;

    protected float currentFovAngle;
    protected float currentFovViewDistance;

    // Provisional Method
    protected Transform playerTransform;

    private bool isAlarmed;
    public bool IsAlarmed { get { return isAlarmed; } }

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();
        npcView = GetComponentInChildren<NPCBaseView>();

        detectionController = GetComponent<NPCDetectionController>();

        if (agent == null)
        {
            Debug.LogWarning("Agent Component Not set");
        }
        if(stateMachine == null)
        {
            Debug.LogWarning("Statem Machine Component Not Set");
        }
        if (npcView == null)
        {
            Debug.LogWarning("NPCView Component Not Set");
        }

        if (detectionController == null)
        {
            Debug.LogWarning("NPCDetectionController Component Not Set");
        }

        // TODO : Pass Player Transform Dynamically
        playerTransform = FindFirstObjectByType<PlayerController>()?.transform;

        SetupStateMachine();

        // Initialize
        npcView.Initialize();
        stateMachine.Initialize();

        currentFovAngle = fovAngle;
        currentFovViewDistance = fovViewDistance;
        npcFov.Initialize(currentFovAngle, currentFovViewDistance);
        detectionController.Initialize(playerTransform.transform, this.transform);


        AlarmOffNPC();

        // Agent Config
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        npcFov.SetAimDirection(GetAimDirection());
        npcFov.SetOrigin(transform.position);
        detectionController.FindPlayer(GetAimDirection(), currentFovViewDistance, currentFovAngle);

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
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
    }

    public bool HasReachedDestination(float thresholdDistance = 0.1f)
    {
        if(!agent.pathPending && agent.remainingDistance < thresholdDistance)
        {
            return true;
        }

        return false;
    }

    private Vector3 GetAimDirection()
    {
        Vector3 direction = agent.velocity.normalized;
        if(direction == Vector3.zero)
        {
            direction = transform.up;
        }
        return direction;
    }

    public virtual void Interact() { }

    public virtual void StopInteract() { }

    public virtual void StopIdleCheck() { }

    public virtual void StopChase() { }
}
