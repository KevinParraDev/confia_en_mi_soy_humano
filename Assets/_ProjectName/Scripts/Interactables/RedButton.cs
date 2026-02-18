using UnityEngine;

public class RedButton : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private WinScreenView winScreen;

    protected override void Awake()
    {
        base.Awake();
        hoverGO?.SetActive(true);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        hoverGO?.SetActive(active);
        hoverGO.GetComponent<Animator>().SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Trun)
            {
                PressButton();
                SoundManager.Instance.PlaySFXByName(Constants.SFX_CLICK);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_ACTION_1, 0.5f);
            }
            else
            {
                Debug.Log("Tú no eres el presidente");
                hoverGO.GetComponent<Animator>().SetTrigger(Constants.ANIM_PANNEL_SHAKE);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_WRONG);
            }
        }
    }
    private void PressButton()
    {
        Debug.Log("EndGame");
        winScreen.Appear();

        SoundManager.Instance.PlaySFXByNameIndex(Constants.SFX_ALERT, 0);
    }
}

