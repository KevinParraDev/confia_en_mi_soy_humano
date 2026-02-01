using UnityEngine;

public class CharacterView : AnimatorControllerBase
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool reversed = false;
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Turn(float x)
    {
        if(!reversed)
        {
            if (x < 0 && transform.localScale.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (x > 0 && transform.localScale.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            if (x < 0 && transform.localScale.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (x > 0 && transform.localScale.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
        }

        
    }

    public void ChangeSkin(RuntimeAnimatorController newSkin)
    {
        animator.runtimeAnimatorController = newSkin;
    }

    public Sprite GetCurrentSprite()
    {
        return spriteRenderer.sprite;
    }
}
