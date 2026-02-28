using System;

[Serializable]
public class Task
{
    public TaskType type;
    public string description;
    public int requiredCount = 1;
    private int currentCount = 0;

    public bool isProgressRequired;

    public bool IsCompleted => currentCount >= requiredCount;

    public Task(TaskType type, string description, int requiredCount = 1)
    {
        this.type = type;
        this.description = description;
        this.requiredCount = requiredCount;
        this.currentCount = 0;
    }

    /// <summary>
    /// Añade progreso a la tarea. No puede superar el requiredCount.
    /// </summary>
    /// <param name="amount">Cantidad a incrementar (por defecto 1)</param>
    public void AddProgress(int amount = 1)
    {
        if (IsCompleted) return;
        currentCount = Math.Min(currentCount + amount, requiredCount);
    }

    public string GetTaskText()
    {
        string progress = IsCompleted ? "Completada" : $"{currentCount}/{requiredCount}";
        
        if (isProgressRequired)
        {
            return $"{description} ({progress})";
        }
        else
        {
            return description;
        }
    }
}
