using UnityEngine;
using UnityEngine.Video;

public class MainState : GameStateBase
{
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private MainController main;
    public void Dependencies()
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        SoundManager.Instance.PlaySongByName(Constants.MUSIC_MAIN, 0);
        main?.Initialize();
        main.PlayActivated += OnPressPlay;
    }
    public void OnPressPlay()
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
        main?.Conclude();
        main.PlayActivated -= OnPressPlay;

        base.ExitState();
    }
}
