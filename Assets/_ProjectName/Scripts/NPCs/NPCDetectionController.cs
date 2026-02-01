using UnityEngine;

public class NPCDetectionController : MonoBehaviour
{
    [SerializeField]
    private LayerMask layer;
    private Transform player;
    private Transform agent;

    private bool isPlayerInRange;
    public bool IsPlayerInRange { get { return isPlayerInRange; } }

    public void Initialize(Transform _player, Transform _agent)
    {
        player = _player;
        agent = _agent;
    }

    public void FindPlayer(Vector3 aimDirection, float detectionRange, float detectionFovAngle)
    {
        if((Vector3.Distance(agent.position,player.position) < detectionRange + 5f)){
            // Player Inside View
            Vector3 directionToPlayer = (player.position - agent.position).normalized;
            float angleBetweenAgentAndPlayer = Vector3.Angle(aimDirection, directionToPlayer);
            if (angleBetweenAgentAndPlayer < detectionFovAngle / 2f)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(agent.position, directionToPlayer, detectionRange, layer);
                Debug.DrawRay(agent.position, directionToPlayer * detectionRange, Color.red);
                if (raycastHit2D.collider != null)
                {

                    if (raycastHit2D.collider.TryGetComponent(out PlayerController _))
                    {
                        // Player Detected
                        if (!isPlayerInRange)
                        {
                            isPlayerInRange = true;
                            Debug.Log("Player Detected by NPC");
                        }
                        return;
                    }
                }
            }
        }

        if (isPlayerInRange)
        {
            isPlayerInRange = false;
        }
    }
}
