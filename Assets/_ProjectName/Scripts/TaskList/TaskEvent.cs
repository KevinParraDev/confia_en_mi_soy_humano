using System;
using UnityEngine;

public abstract class TaskEvent { }

// Eventos para las tareas
public class LocationReachedEvent : TaskEvent { public MapZone locationToReach; }
public class MealCookedEvent : TaskEvent { }
public class NPCInteractionEvent : TaskEvent { public NPC npcToInteract; }

public static class TaskEventManager
{
    public static Action<TaskEvent> OnTaskEventRaised;

    public static void RaiseEvent(TaskEvent taskEvent)
    {
        OnTaskEventRaised?.Invoke(taskEvent);
    }
}

