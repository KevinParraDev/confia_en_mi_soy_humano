using UnityEngine;

public class PotController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private DishesDeskInteractable dishDesk;

    protected override void Awake()
    {
        base.Awake();
        hoverGO?.SetActive(true);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        view?.Hover(active);
        hoverGO.GetComponent<Animator>().SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            if(playerController.GetCurrentSkin() == NPC.Chef)
            {
                Cook();
            }
            else
            {
                Debug.Log("Solo puedes cocinar si eres chef");
                hoverGO.GetComponent<Animator>().SetTrigger(Constants.ANIM_PANNEL_SHAKE);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_WRONG);
            }
        }
    }
    private void Cook()
    {
        Debug.Log("Cocinar");
        dishDesk.AddDish();
    }
}
