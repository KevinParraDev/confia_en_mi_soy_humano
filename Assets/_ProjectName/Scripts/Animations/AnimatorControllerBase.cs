using UnityEngine;

public abstract class AnimatorControllerBase : MonoBehaviour
{
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator not found on " + gameObject.name);
    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
            animator.SetTrigger(animationName);
        else
            Debug.LogError("Animator is not assigned for " + gameObject.name);
    }

    public void SetBoolAnimation(string parameterName, bool value)
    {
        if (animator != null)
            animator.SetBool(parameterName, value);
        else
            Debug.LogError("Animator is not assigned for " + gameObject.name);
    }

    public void SetFloatAnimation(string parameterName, float value)
    {
        if (animator != null)
            animator.SetFloat(parameterName, value);
        else
            Debug.LogError("Animator is not assigned for " + gameObject.name);
    }

    public void SetIntAnimation(string parameterName, int value)
    {
        if (animator != null)
            animator.SetInteger(parameterName, value);
        else
            Debug.LogError("Animator is not assigned for " + gameObject.name);
    }

    public float GetAnimationLenght()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void ResetTriggerAnimatorParameters()
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                animator.ResetTrigger(parameter.name);
            }
        }
        else
        {
            Debug.LogError("Animator is not assigned for " + gameObject.name);
        }
    }
    public void ResetBoolAnimatorParameters()
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                animator.SetBool(parameter.name, false);
            }
        }
        else
        {
            Debug.LogError("Animator is not assigned for " + gameObject.name);
        }
    }
}
