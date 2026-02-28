using DG.Tweening;
using UnityEngine;

public class RoomMaskController : MonoBehaviour
{
    private SpriteMask spriteMask;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color lightOffColor;
    [SerializeField] private Color lightOnColor;

    private void Awake()
    {
        spriteMask = GetComponent<SpriteMask>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.enabled = false;
        spriteMask.enabled = false;
        spriteRenderer.color = lightOffColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out _))
        {
            RoomFocusManager.Instance.RegisterEnter(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out _))
        {
            RoomFocusManager.Instance.RegisterExit(this);
        }
    }

    public void LightOn()
    {
        spriteRenderer.enabled = true;
        spriteMask.enabled = true;

        spriteRenderer.DOKill();
        spriteRenderer.DOColor(lightOnColor, 0.5f);
    }

    public void LightOff()
    {
        spriteRenderer.DOKill();
        spriteRenderer.DOColor(lightOffColor, 0.5f)
            .OnComplete(() =>
            {
                spriteMask.enabled = false;
                spriteRenderer.enabled = false;
            });
    }
}