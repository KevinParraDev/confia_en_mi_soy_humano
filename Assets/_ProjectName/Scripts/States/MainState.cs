using UnityEngine;
using UnityEngine.Video;

public class MainState : GameStateBase
{
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private MainController main;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private OnboardingVideoController onboardingVideoController;
    public void Dependencies()
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        SoundManager.Instance.PlaySongByName(Constants.MUSIC_MAIN, 0);
        settingsManager?.Initialize();
        main?.Initialize();
        main.PlayActivated += OnPressPlay;
        onboardingVideoController.Ended += StartGame;
        onboardingVideoController.Initialize();
    }
    public void OnPressPlay()
    {
        video.Play();
        video.loopPointReached += OnVideoFinished;
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        StartGame();
    }
    private void StartGame()
    {
        nextState = States.KevinScene;
        ExitState();
    }
    public override void ExitState()
    {
        settingsManager?.Conclude();
        main?.Conclude();
        main.PlayActivated -= OnPressPlay;
        onboardingVideoController.Ended -= StartGame;
        onboardingVideoController.Conclude();

        base.ExitState();
    }
}
