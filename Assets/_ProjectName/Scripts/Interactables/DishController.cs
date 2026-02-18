using UnityEngine;

public class DishController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private bool poison = false;
    protected override void Awake()
    {
        base.Awake();
        hoverGO.SetActive(true);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        view?.Hover(active);
        //hoverGO.SetActive(active);
        hoverGO.GetComponent<Animator>().SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Waiter)
            {
                Debug.Log("Tomar plato");
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Solo puedes llevar platos si eres mesero");
                hoverGO.GetComponent<Animator>().SetTrigger(Constants.ANIM_PANNEL_SHAKE);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_WRONG);
            }
        }
    }
    public override void Transform(PlayerController playerController = null)
    {
        base.Transform();
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Waiter)
            {
                poison = true;
                Debug.Log("Envenenar");
            }
            else
            {
                Debug.Log("No eres mesero");
            }
        }
    }
}

