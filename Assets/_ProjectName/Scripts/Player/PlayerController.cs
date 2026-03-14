using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementController movementController;
    private PlayerInteractableController interactableController;
    private CharacterView view;
    [SerializeField] private NpcDataSO currentNPC;
    [SerializeField] private bool walkInAwake = false;

    private bool onboardingSeen;
    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        interactableController = GetComponentInChildren<PlayerInteractableController>();
        view = GetComponentInChildren<CharacterView>();
    }
    public void Initialize()
    {
        if(walkInAwake)
        {
            interactableController?.Initialize();
            movementController?.Initialize();
        }
    }
    public void EnableInteract()
    {
        interactableController?.Initialize();
    }
    public void ChangeSkin(NpcDataSO npc)
    {
        Debug.Log("Player Transform 4");
        if (!onboardingSeen)
        {
            onboardingSeen = true;
            if (!walkInAwake)
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
