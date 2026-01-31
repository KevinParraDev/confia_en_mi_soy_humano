using UnityEngine;

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
        return currentNPC != null ? currentNPC.npcType : NPC.Alien;
    }
    public void Conclude()
    {
        movementController?.Conclude();
        interactableController?.Conclude();
    }
}
