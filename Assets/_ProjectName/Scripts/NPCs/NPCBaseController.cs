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

    private bool isAlarmed;
    public bool IsAlarmed { get { return isAlarmed; } }

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = GetComponent<StateMachine>();
        npcView = GetComponentInChildren<NPCBaseView>();

        if(agent == null)
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

        SetupStateMachine();

        // Initialize
        npcView.Initialize();
        stateMachine.Initialize();

        AlarmOffNPC();

        // Agent Config
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
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
    }

    public void StopMovement()
    {
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

    public virtual void Interact() { }

    public virtual void StopInteract() { }

    public virtual void StopIdleCheck() { }
}
