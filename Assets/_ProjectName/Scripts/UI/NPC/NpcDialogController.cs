using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcDialogController : AnimatorControllerBase
{
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Image playerIcon;

    private bool open = false;
    public void ShowDialog(Sprite icon, string dialog)
    {
        playerIcon.sprite = icon;
        dialogText.text = dialog;

        open = !open;
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, open);
    }

    public void Close()
    {
        open = false;
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, false);
    }
}
