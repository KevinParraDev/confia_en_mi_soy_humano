using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCanvasController : AnimatorControllerBase
{
    public void SetActiveButtons(bool active)
    {
        Debug.Log("Set active buttons " + active);
        SetBoolAnimation(Constants.ANIM_PANNEL_APPEAR, active);
    }
}
