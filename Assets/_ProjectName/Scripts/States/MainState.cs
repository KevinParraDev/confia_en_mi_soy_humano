using UnityEngine;
using UnityEngine.Video;

public class MainState : GameStateBase
{
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private MainController main;
    [SerializeField] private VideoPlayer video;
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
    }
    public void OnPressPlay()
    {
        //nextState = States.KevinScene;
        //ExitState();

        video.Play();
        video.loopPointReached += OnVideoFinished;
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        nextState = States.KevinScene;
        ExitState();
    }
    public override void ExitState()
    {
        base.ExitState();

        settingsManager?.Conclude();
        main?.Conclude();
        main.PlayActivated -= OnPressPlay;
    }

    //TODO:DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            nextState = States.Onboarding;
            ExitState();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {

        }
    }
}
