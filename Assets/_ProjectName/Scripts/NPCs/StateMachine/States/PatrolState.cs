using UnityEngine;

public class PatrolState : IState
{
    private NPCBaseController npc;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float waitTimeAtPoint = 2f;
    private float timer = 0f;
    private bool isWaiting = false;
    private bool isAlwaysPatrolling = false;

    public PatrolState(NPCBaseController npc, Transform[] waypoints, float waitTime = 2f, bool isAlwaysPatrolling = false)
    {
        this.npc = npc;
        this.waypoints = waypoints;
        this.waitTimeAtPoint = waitTime;
        this.isAlwaysPatrolling = isAlwaysPatrolling;
    }

    public void Enter()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            currentWaypointIndex = 0;
            MoveToWaypoint();
        }
    }

    public void Execute()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (isWaiting)
        {
            timer += Time.deltaTime;
            if (timer >= waitTimeAtPoint)
            {
                isWaiting = false;
                NextWaypoint();
                return;
            }
            npc.StopMovement();
        }
        else if (npc.HasReachedDestination())
        {
            isWaiting = true;
            timer = 0f;
        }
    }

    public void Exit() { }

    private void MoveToWaypoint()
    {
        npc.SetNewDestination(waypoints[currentWaypointIndex].position);
        npc.ResumeMovement();
    }

    private void NextWaypoint()
    {
        if(isAlwaysPatrolling)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            MoveToWaypoint();
            return;
        }

        if(currentWaypointIndex == waypoints.Length - 1)
        {
            npc.BackToIdle();
            return;
        }

        currentWaypointIndex++;
        MoveToWaypoint();
    }

    public void ResetPatrol()
    {
        currentWaypointIndex = 0;
        MoveToWaypoint();
    }

    public int GetCurrentWaypointIndex()
    {
        return currentWaypointIndex;
    }
}
