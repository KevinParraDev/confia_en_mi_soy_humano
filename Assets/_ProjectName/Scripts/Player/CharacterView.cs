using UnityEngine;

public class CharacterView : AnimatorControllerBase
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool reversed = false;

    private Material outlineMaterial;
    private const string outlineColorProp = "_OutlineColor";
    private const string outlineThicknessProp = "_OutlineThickness";
    private const float outlineThickness = 5.0f;
    [SerializeField] private Color outlineColor;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            outlineMaterial = spriteRenderer.material;
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

    public void Hover(bool active)
    {
        if (outlineMaterial == null)
            return;

        outlineMaterial.SetFloat(outlineThicknessProp, active ? outlineThickness : 0.0f);
        outlineMaterial.SetColor(outlineColorProp, active ? outlineColor : Color.black);

        if (active)
            SoundManager.Instance.PlaySFXByName(Constants.SFX_HOVER);
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
