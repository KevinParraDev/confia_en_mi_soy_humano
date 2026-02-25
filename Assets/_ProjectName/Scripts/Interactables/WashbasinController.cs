using TMPro;
using UnityEngine;

public class WashbasinController : InteractableBaseController
{
    [SerializeField] private GameObject hoverGO;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text OpenCloseText;
    private bool open = false;

    protected override void Awake()
    {
        base.Awake();
        hoverGO?.SetActive(true);
        OpenCloseText.text = "Abrir";
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
            Toogle();
        }
    }
    private void Toogle()
    {
        open = !open;
        animator.SetBool(Constants.ANIM_PANNEL_APPEAR, open);
        OpenCloseText.text = open ? "Cerrar" : "Abrir";
    }
}
