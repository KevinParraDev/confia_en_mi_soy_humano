using System;
using UnityEngine;

public class OvniButtonController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private Animator panelInfoAnim;
    [SerializeField] private OnboardingState onboardingState;

    public event Action Failed;
    private bool initialized = false;

    protected override void Awake()
    {
        base.Awake();
        hoverGO?.SetActive(false);
    }
    public void Initialized()
    {
        initialized = true;
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        if(!initialized)
            return;

        hoverGO?.SetActive(active);
        panelInfoAnim.SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (!initialized)
            return;

        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Alien)
            {
                PressButton();
                SoundManager.Instance.PlaySFXByName(Constants.SFX_CLICK);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_ACTION_1, 0.5f);
            }
            else
            {
                Debug.Log("Tú no eres el presidente");
                panelInfoAnim.GetComponent<Animator>().SetTrigger(Constants.ANIM_PANNEL_SHAKE);
                SoundManager.Instance.PlaySFXByName(Constants.SFX_WRONG);

                Failed?.Invoke();
            }
        }
    }
    private void PressButton()
    {
        Debug.Log("EndGame");
        onboardingState.OnPressButton();

        SoundManager.Instance.PlaySFXByNameIndex(Constants.SFX_ALERT, 0);
    }
}
