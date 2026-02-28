using UnityEngine;
using UnityEngine.EventSystems;

public class WinScreenView : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject homeButton;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void OnAppearButton()
    {
        EventSystem.current.SetSelectedGameObject(homeButton);
    }
    public void Appear()
    {
        anim.SetTrigger(Constants.ANIM_PANNEL_APPEAR);
    }
    public void PlayExplotionSound()
    {
        SoundManager.Instance.PlaySFXByName(Constants.SFX_EXPLOTION);
        SoundManager.Instance.PlaySongByName(Constants.MUSIC_SPACE, 0);
    }
}
