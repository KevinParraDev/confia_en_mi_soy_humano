using UnityEditor.Animations;
using UnityEngine;

public class CharacterView : AnimatorControllerBase
{
    public void Turn(float x)
    {
        if (x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (x > 0 && transform.localScale.x < 0)
            transform.localScale = Vector3.one;
    }

    public void ChangeSkin(RuntimeAnimatorController newSkin)
    {
        animator.runtimeAnimatorController = newSkin;
    }
}
