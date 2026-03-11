using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        if(homeButton == null)
            homeButton = GetComponentInChildren<Button>().gameObject;
        EventSystem.current.SetSelectedGameObject(homeButton);
    }
    public void Appear()
    {
        gameObject.SetActive(true);
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
