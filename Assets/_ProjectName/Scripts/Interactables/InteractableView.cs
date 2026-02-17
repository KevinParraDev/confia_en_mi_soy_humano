using UnityEngine;

public class InteractableView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material outlineMaterial;

    private const string outlineColorProp = "_OutlineColor";
    private const string outlineThicknessProp = "_OutlineThickness";

    private const float outlineThickness = 3.0f;
    [SerializeField] private Color outlineColor;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            outlineMaterial = spriteRenderer.material;
    }

    public void Hover(bool active)
    {
        if (outlineMaterial == null)
            return;

        outlineMaterial.SetFloat(outlineThicknessProp, active ? outlineThickness : 0.0f);
        outlineMaterial.SetColor(outlineColorProp, active ? outlineColor : Color.black);
        SoundManager.Instance.PlaySFXByName(Constants.SFX_HOVER);
    }
}
