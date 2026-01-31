using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;

    private int currentPatrolIndex;
    private NavMeshAgent agent;

    public void Initialize()
    {
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned.");
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing.");
            return;
        }

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = patrolSpeed;
        currentPatrolIndex = 0;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

    }

    public void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

}
