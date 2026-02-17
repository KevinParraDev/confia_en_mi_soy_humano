using UnityEngine;

public class RedButton : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private WinScreenView winScreen;

    protected override void Awake()
    {
        base.Awake();
        hoverGO?.SetActive(false);
    }
    public override void Hover(bool active, PlayerController playerController = null)
    {
        hoverGO?.SetActive(active);
    }

    public override void Interact(PlayerController playerController = null)
    {
        if (playerController != null)
        {
            if (playerController.GetCurrentSkin() == NPC.Trun)
            {
                PressButton();
            }
            else
            {
                Debug.Log("Tú no eres el presidente");
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

