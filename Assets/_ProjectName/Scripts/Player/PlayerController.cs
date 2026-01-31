using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController movementController;
    private PlayerInteractableController interactableController;
    private CharacterView view;

    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        interactableController = GetComponentInChildren<PlayerInteractableController>();
        view = GetComponentInChildren<CharacterView>();
    }
    public void Initialize()
    {
        movementController?.Initialize();
        interactableController?.Initialize();
    }
    public void ChangeSkin(RuntimeAnimatorController newSkin)
    {
        view.ChangeSkin(newSkin);
    }
    public void Conclude()
    {
        movementController?.Conclude();
        interactableController?.Conclude();
    }
}
