using UnityEngine;

public class KevinTestState : GameStateBase
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AlarmBarController alarmBarController;
    [SerializeField] private LoseScreenView loseScreen;
    [SerializeField] private NPCBaseController[] npcsAgentList;
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
        alarmBarController?.Initialize();
        //foreach (var npc in npcsAgentList)
        //    npc.Initialize(playerController);
    }

    public void GoHome()
    {
        nextState = States.Main;
        ExitState();
    }

    public void GameOver()
    {
        loseScreen.Appear();
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
