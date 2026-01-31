using UnityEngine;

public class NPCDetectionController : MonoBehaviour
{
    
    private float detectionFov;
    private float detectionRange;
    private Transform player;
    private Transform agent;

    private bool isPlayerInRange;
    public bool IsPlayerInRange { get { return isPlayerInRange; } }

    public void Initialize(float fov, float range, Transform _player, Transform _agent)
    {
        detectionFov = fov;
        detectionRange = range;
        player = _player;
        agent = _agent;
    }

    public void FindPlayer(Vector3 aimDirection)
    {
        if((Vector3.Distance(agent.position,player.position) < detectionRange)){
            // Player Inside View
            Vector3 directionToPlayer = (player.position - agent.position).normalized;
            float angleBetweenAgentAndPlayer = Vector3.Angle(aimDirection, directionToPlayer);
            if(angleBetweenAgentAndPlayer < detectionFov / 2f)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(agent.position, directionToPlayer, detectionRange);
                
                if (raycastHit2D.collider != null)
                {
                    if(raycastHit2D.collider.TryGetComponent(out PlayerController _))
                    {
                        // Player Detected
                        if (!isPlayerInRange)
                        {
                            isPlayerInRange = true;
                            Debug.Log("Player Is in range");
                        }
                        return;
                    }
                }
            }
        }

        if (isPlayerInRange)
        {
            isPlayerInRange = false;
            Debug.Log("Player Out Of view");
        }
    }
}
