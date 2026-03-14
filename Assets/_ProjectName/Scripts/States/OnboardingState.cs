
using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

public class OnboardingState : GameStateBase
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private OnboardingVideoController onboardingVideoController;
    [SerializeField] private CanvasGroup videoCG;
    protected override void Awake()
    {
        base.Awake();
    }

    public void Dependencies()
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        playerController?.Initialize();
        SoundManager.Instance.PlaySongByName(Constants.MUSIC_SONG_2, 0, 1, 1);

        onboardingVideoController.Ended += StartGameplay;
    }
    public void OnPressButton()
    {
        Debug.Log("Press button");

        videoCG.DOFade(1, 1f);
        onboardingVideoController.Initialize();
        video.Play();
        video.loopPointReached += OnVideoFinished;
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        StartGameplay();
    }
    public void StartGameplay()
    {
        nextState = States.KevinScene;
        ExitState();
    }
    public override void ExitState()
    {
        playerController?.Conclude();
        onboardingVideoController.Ended -= StartGameplay;
        onboardingVideoController.Conclude();

        base.ExitState();
    }
}
