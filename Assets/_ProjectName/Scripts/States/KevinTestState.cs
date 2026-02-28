using UnityEngine;

public class KevinTestState : GameStateBase
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AlarmBarController alarmBarController;
    protected override void Awake()
    {
        base.Awake();
    }

    private void DefaultValues()
    {

    }

    public void Dependencies()
    {
        DefaultValues();
    }

    public override void EnterState()
    {
        base.EnterState();
        Initialize();
    }
    public void Initialize()
    {
        SoundManager.Instance.PlaySongByName(Constants.MUSIC_SONG_2, 0, 1, 1);

        //playerController?.Initialize();
        alarmBarController?.Initialize();
    }

    public void GoHome()
    {
        nextState = States.Main;
        ExitState();
    }

    private void GameOver()
    {

    }
    public void Conclude()
    {
        playerController?.Conclude();
        alarmBarController?.Conclude();
    }

    public override void ExitState()
    {
        Conclude();
        base.ExitState();
    }
}
