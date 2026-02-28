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
        }

        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log($"Player entered {zone} zone.");

            switch (zone)
            {
                case MapZone.ConferenceRoom:
                    // Update Task Progress
                    TaskListManager.Instance?.OnTaskEvent(TaskType.EntrarSalaConferencias, 1);

                    // Update Task To Next Task
                    TaskListManager.Instance?.OnUpdateNextTask(TaskType.AislarPresidente);
                    break;
                case MapZone.PresidentRoom:
                    // Update Task Progress
                    TaskListManager.Instance?.OnTaskEvent(TaskType.SalaPresidencial, 1);

                    // Update Task To Next Task
                    TaskListManager.Instance?.OnUpdateNextTask(TaskType.PrepararPlaneta);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC")) {
            zoneCurrentNPCs--;

            if(zone == MapZone.ConferenceRoom && zoneCurrentNPCs == 0)
            {
                // Update Task Progress
                TaskListManager.Instance?.OnTaskEvent(TaskType.AislarPresidente, 1);

                // Update Task To Next Task
                TaskListManager.Instance?.OnUpdateNextTask(TaskType.SalaPresidencial);
            }
        }

        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = false;
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
