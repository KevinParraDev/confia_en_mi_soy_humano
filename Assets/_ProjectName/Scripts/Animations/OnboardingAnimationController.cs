using UnityEngine;

public class OnboardingAnimationController : MonoBehaviour
{
    [SerializeField] private NpcInteractableController chefInteractable;
    [SerializeField] private CharacterView chefView;
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] protected GameObject alienBox;
    [SerializeField] protected GameObject normalBox;
    [SerializeField] protected PlayerInteractableController playerInteractable;

    public void StartWalk()
    {
        chefView.SetBoolAnimation(Constants.ANIM_MOVING, true);
    }

    public void StartConversation()
    {
        chefView.SetBoolAnimation(Constants.ANIM_MOVING, false);
        chefInteractable.StartDialogue(NPC.Alien);
        playerInteractable.SkinChanged += EndConversation;
    }
    
    public void EndConversation()
    {
        playerInteractable.SkinChanged -= EndConversation;
        alienBox.SetActive(false);
        normalBox.SetActive(true);
        playerRenderer.enabled = true;
        player?.Initialize();
    }


}
