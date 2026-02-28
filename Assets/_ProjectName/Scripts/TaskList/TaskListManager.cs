using System.Collections.Generic;
using UnityEngine;


// TaskType.cs
public enum TaskType
{
    Infiltrar,              // requiere 1
    CocinarPlatos,          // requiere 3
    AsimilarBurocrata,      // requiere 1
    EntrarSalaConferencias, // requiere 1
    AislarPresidente,       // requiere 1
    SalaPresidencial,       // requiere 1
    PrepararPlaneta         // requiere 1
}

public class TaskListManager : MonoBehaviour
{
    public static TaskListManager Instance { get; private set; }

    [SerializeField] private List<Task> tasks = new List<Task>();

    private int currentTaskIndex = 0;

    private TaskListView taskListView;

    private void Awake()
    {
        taskListView = GetComponentInChildren<TaskListView>();
        if (taskListView == null)
        {
            Debug.LogError("TaskListView no encontrado en el mismo GameObject que TaskListManager");
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional, si quieres que persista entre escenas
            InitializeTasks();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeTasks()
    {
        // Aquí se crean las tareas con sus descripciones y requerimientos
        //tasks = new List<Task>
        //{
        //    new Task(TaskType.CocinarPlatos, "Cocinar 2 platos", 2),
        //    new Task(TaskType.AsimilarBurocrata, "Asimilar a un burócrata", 1),
        //    new Task(TaskType.EntrarSalaConferencias, "Entrar a la sala de conferencias", 1),
        //    new Task(TaskType.AislarPresidente, "Aislar al presidente", 1),
        //    new Task(TaskType.PrepararPlaneta, "Preparar el planeta", 1)
        //};

        // Actualiza la vista con la primera tarea
        if (tasks.Count > 0)
        {
            taskListView.UpdateTask(tasks[0].GetTaskText());
        }
        else
        {
            Debug.LogWarning("No se han definido tareas en TaskListManager");
            return; // No hay tareas para mostrar
        }
    }

    /// <summary>
    /// Llama a este método cuando ocurra una acción relevante para las tareas.
    /// </summary>
    /// <param name="type">Tipo de tarea que se debe avanzar</param>
    /// <param name="amount">Cantidad de progreso (por defecto 1)</param>
    public void OnTaskEvent(TaskType type, int amount = 1)
    {
        if (currentTaskIndex >= tasks.Count)
        {
            Debug.LogWarning("Todas las tareas ya están completadas");
            return;
        }

        Task currentTask = tasks[currentTaskIndex];
        if (currentTask == null)
        {
            Debug.LogWarning($"No se encontró tarea en el índice {currentTaskIndex}");
            return;
        }

        if (currentTask.type == type)
        {
            currentTask.AddProgress(amount);
            taskListView.UpdateTask(currentTask.GetTaskText());

            if (currentTask.IsCompleted)
            {
                
            }
        }
        else
        {
            Debug.LogWarning($"Evento de tarea recibido para {type}, pero la tarea actual es {currentTask.type}");
        }
    }

    public void OnUpdateNextTask(TaskType newTask)
    {
        if (currentTaskIndex >= tasks.Count)
        {
            Debug.Log("¡Todas las tareas completadas!");
            return;
        }

        if (tasks[currentTaskIndex + 1].type == newTask)
        {
            currentTaskIndex++;
            taskListView.UpdateTask(tasks[currentTaskIndex].GetTaskText());
        }
    }

    /// <summary>
    /// Devuelve la lista completa de tareas (solo lectura para la vista)
    /// </summary>
    public IReadOnlyList<Task> GetTasks() => tasks.AsReadOnly();
}
