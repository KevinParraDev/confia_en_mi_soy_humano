using UnityEngine;

public class TrunDoorController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private GameObject doorGO;
    protected override void Awake()
    {
        base.Awake();
        hoverGO.SetActive(true);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        hoverGO.GetComponent<Animator>().SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Trun)
            {
                Debug.Log("Abrir puerta");
                doorGO?.SetActive(false);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_ACTION_1);
            }
            else
            {
                Debug.Log("Solo puedes llevar platos si eres mesero");
                hoverGO.GetComponent<Animator>().SetTrigger(Constants.ANIM_PANNEL_SHAKE);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_WRONG);
            }
        }
    }
}
