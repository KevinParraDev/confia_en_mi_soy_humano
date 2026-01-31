using Unity.Cinemachine;
using UnityEngine;

public class RoomMaskController : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;
    private SpriteMask spriteMask;
    private void Awake()
    {
        spriteMask = GetComponent<SpriteMask>();
        cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out _))
        {
            spriteMask.enabled = true;
            cinemachineCamera.Follow = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out _))
        {
            spriteMask.enabled = false;
        }
    }
}
