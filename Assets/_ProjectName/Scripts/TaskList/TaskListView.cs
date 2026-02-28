using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TaskListView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text currentTaskText;
    [SerializeField] private Image mapImage;
    private bool isOpen = false;

    private PlayerInput playerInput;
    private void Awake()
    {
        isOpen = false;
        animator.SetBool(Constants.ANIM_PANNEL_APPEAR, false);

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
        isOpen = !isOpen;
        animator.SetBool(Constants.ANIM_PANNEL_APPEAR, isOpen);
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
