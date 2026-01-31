using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController movementController;
    private PlayerInteractableController interactableController;
    private CharacterView view;
    [SerializeField] private NpcDataSO currentNPC;
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
    public void ChangeSkin(NpcDataSO npc)
    {
        currentNPC = npc;
        view.ChangeSkin(currentNPC.animatorSkin);
    }
    public NPC GetCurrentSkin()
    {
        return currentNPC != null ? currentNPC.npcType : NPC.None;
    }
    public void Conclude()
    {
        movementController?.Conclude();
        interactableController?.Conclude();
    }
}
