using System;
using UnityEngine;

public class ZoneDetectionController : MonoBehaviour
{
    [SerializeField]
    private MapZone zone;

    [SerializeField]
    private int zoneCurrentNPCs = 0;

    [SerializeField]
    private bool isPlayerInZone = false;

    public static Action<int, SuspicionType> onPlayerDetected;

    private void Awake()
    {
        NpcInteractableController.onNPCIsCopied += OnPlayerUseCopy;
    }

    private void OnDestroy()
    {
        NpcInteractableController.onNPCIsCopied -= OnPlayerUseCopy;
    }

    private void OnPlayerUseCopy()
    {
        if (zoneCurrentNPCs > 1 && isPlayerInZone)
        {
            onPlayerDetected?.Invoke(100, SuspicionType.VisibleTransformation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            zoneCurrentNPCs++;
            Debug.Log($"NPC entered {zone} zone. Current NPCs: {zoneCurrentNPCs}");
        }

        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log($"Player entered {zone} zone.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC")) {
            zoneCurrentNPCs--;
            Debug.Log($"NPC exited {zone} zone. Current NPCs: {zoneCurrentNPCs}");
        }

        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = false;
            Debug.Log($"Player exited {zone} zone.");
        }
    }
}

public enum MapZone
{
    Storage,
    Kitchen,
    MainHall,
    ConferenceRoom,
    PresidentRoom,
    Bathroom
}
