using TMPro;
using UnityEngine;

public class TaskListView : MonoBehaviour
{
    [SerializeField] private TMP_Text currentTaskText;

    public void UpdateTask(string taskText)
    {
        if (currentTaskText != null)
        {
            currentTaskText.text = taskText;
        }
        else
        {
            Debug.LogError("currentTaskText no asignado en TaskListView");
        }
    }
}
