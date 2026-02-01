using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
    [SerializeField] private Animator buttonsAnim;
    [SerializeField] private Animator changeSkinAnim;
    [SerializeField] private Image maskIcon;
    public void SetActiveButtons(bool active, Sprite icon)
    {
        maskIcon.sprite = icon;
        buttonsAnim.SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }
    public void PlayChangeSkinAnimation()
    {
        changeSkinAnim.SetTrigger(Constants.ANIM_PANNEL_APPEAR);
    }
}
