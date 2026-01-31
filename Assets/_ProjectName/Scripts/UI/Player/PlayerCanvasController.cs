using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCanvasController : MonoBehaviour
{
    [SerializeField] private Animator buttonsAnim;
    [SerializeField] private Animator changeSkinAnim;
    public void SetActiveButtons(bool active)
    {
        buttonsAnim.SetBool(Constants.ANIM_PANNEL_APPEAR, active);
    }
    public void PlayChangeSkinAnimation()
    {
        changeSkinAnim.SetTrigger(Constants.ANIM_PANNEL_APPEAR);
    }
}
