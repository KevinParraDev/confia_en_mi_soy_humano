using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class RoomMaskController : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;
    private SpriteMask spriteMask;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color lightOffColor;
    [SerializeField] private Color lightOnColor;
    private void Awake()
    {
        spriteMask = GetComponent<SpriteMask>();
        cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;
        spriteMask.enabled = false;
        spriteRenderer.color = lightOffColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out _))
        {
            spriteRenderer.enabled = true;
            spriteMask.enabled = true;
            cinemachineCamera.Follow = transform;
            spriteRenderer.DOColor(lightOnColor, 0.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out _))
        {
            spriteRenderer.DOColor(lightOffColor, 0.5f).OnComplete(() =>
            {
                spriteMask.enabled = false;
                spriteRenderer.enabled = false;
            });
        }
    }
}
