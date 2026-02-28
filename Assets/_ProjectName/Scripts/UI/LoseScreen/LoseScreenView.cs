using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LoseScreenView : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private PlayerInput playerInput;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerInput = FindAnyObjectByType<PlayerInput>();
    }
    public void Appear()
    {
        anim.SetTrigger(Constants.ANIM_PANNEL_APPEAR);
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");

        SoundManager.Instance.PlaySongByName(Constants.MUSIC_GAMEOVER, 0);
    }
    public void OnAppearButton()
    {
        EventSystem.current.SetSelectedGameObject(homeButton);
    }
}
