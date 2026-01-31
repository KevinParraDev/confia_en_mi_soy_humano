using UnityEngine;

public class PatrolState : IState
{
    private NPCBaseController npc;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float waitTimeAtPoint = 2f;
    private float timer = 0f;
    private bool isWaiting = false;

    public PatrolState(NPCBaseController npc, Transform[] waypoints, float waitTime = 2f)
    {
        this.npc = npc;
        this.waypoints = waypoints;
        this.waitTimeAtPoint = waitTime;
    }

    public void Enter()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
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
            }
        }
        else if (npc.HasReachedDestination())
        {
            isWaiting = true;
            timer = 0f;
            OnReachedWaypoint();
        }
    }

    public void Exit() { }

    private void MoveToWaypoint()
    {
        npc.SetNewDestination(waypoints[currentWaypointIndex].position);
    }

    private void NextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        MoveToWaypoint();
    }

    private void OnReachedWaypoint()
    {
        npc.Interact();
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
