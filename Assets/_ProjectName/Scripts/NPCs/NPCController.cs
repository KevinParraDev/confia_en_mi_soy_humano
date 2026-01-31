using UnityEngine;

public class NPCController : MonoBehaviour
{
    private NPCPatrol npcPatrol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcPatrol = GetComponent<NPCPatrol>();
        if (npcPatrol == null)
        {
            Debug.LogError("NPCPatrol component missing.");
            return;
        }
        npcPatrol.Initialize();
    }

}
