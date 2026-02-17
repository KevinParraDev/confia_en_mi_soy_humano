using UnityEngine;

public class WinScreenView : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
