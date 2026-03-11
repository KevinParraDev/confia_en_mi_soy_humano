using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        gameObject.SetActive(true);
        anim.SetTrigger(Constants.ANIM_PANNEL_APPEAR);
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap("UI");

        SoundManager.Instance.PlaySongByName(Constants.MUSIC_GAMEOVER, 0);
    }
    public void OnAppearButton()
    {
        if (homeButton == null)
            homeButton = GetComponentInChildren<Button>().gameObject;
        EventSystem.current.SetSelectedGameObject(homeButton);
    }
}
