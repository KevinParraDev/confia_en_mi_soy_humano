using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController movementController;
    private PlayerInteractableController interactableController;
    private CharacterView view;
    [SerializeField] private NpcDataSO currentNPC;

    private bool onboardingSeen;
    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        interactableController = GetComponentInChildren<PlayerInteractableController>();
        view = GetComponentInChildren<CharacterView>();
    }
    public void Initialize()
    {
        interactableController?.Initialize();
    }
    public void ChangeSkin(NpcDataSO npc)
    {
        if(!onboardingSeen)
        {
            onboardingSeen = true;
            movementController?.Initialize();
        }
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
