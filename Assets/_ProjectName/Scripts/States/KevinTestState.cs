using UnityEngine;

public class KevinTestState : GameStateBase
{
    [SerializeField] private PlayerController playerController;
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
        playerController?.Initialize();

    }

    private void StartGameplay()
    {

    }

    private void GameOver()
    {

    }
    public void Conclude()
    {
        playerController?.Conclude();
    }

    public override void ExitState()
    {
        Conclude();
        base.ExitState();
    }
}
