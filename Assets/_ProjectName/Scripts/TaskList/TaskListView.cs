using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TaskListView : MonoBehaviour
{
    [SerializeField] private TMP_Text currentTaskText;
    [SerializeField] private Image mapImage;

    private PlayerInput playerInput;
    private void Awake()
    {
        HideMap();

        playerInput = FindAnyObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["OpenMap"].performed += ToggleMap;
        }
    }

    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions["OpenMap"].performed -= ToggleMap;
        }   
    }

    private void ToggleMap(InputAction.CallbackContext context)
    {
        if (mapImage.rectTransform.localScale == Vector3.zero)
        {
            ShowMap();
        }
        else
        {
            HideMap();
        }
    }

    private void ShowMap()
    {
        mapImage.rectTransform.localScale = Vector3.one; // Start with the map hidden
    }

    private void HideMap()
    {
        mapImage.rectTransform.localScale = Vector3.zero; // Start with the map hidden
    }

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
