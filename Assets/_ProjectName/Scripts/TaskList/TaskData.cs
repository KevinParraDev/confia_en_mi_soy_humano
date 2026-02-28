using UnityEngine;

public abstract class TaskData : ScriptableObject
{
    [SerializeField] private string _taskName;
    public string TaskName => _taskName;
    [SerializeField] private string _description;
    public string Description => _description;

    public abstract Task CreateInstance();
}
