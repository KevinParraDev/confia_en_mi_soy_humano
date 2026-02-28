using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WinScreenView : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private PlayerInput playerInput;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInput = FindAnyObjectByType<PlayerInput>();
    }
    public void OnAppearButton()
    {
        EventSystem.current.SetSelectedGameObject(homeButton);
    }
    public void Appear()
    {
        anim.SetTrigger(Constants.ANIM_PANNEL_APPEAR);
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");
    }
    public void PlayExplotionSound()
    {
        SoundManager.Instance.PlaySFXByName(Constants.SFX_EXPLOTION);
        SoundManager.Instance.PlaySongByName(Constants.MUSIC_SPACE, 0);
    }
}
