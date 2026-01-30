using UnityEngine;

public class GameplayState : GameStateBase
{
    private bool isGameplayActive = false;

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
    }


    private void StartGameplay()
    {

    }

    private void GameOver()
    {

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    //TODO:DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Globals.InputDebugModeActivated && !isGameplayActive)
                StartGameplay();
            else
                Debug.LogError("You need to activate Debug mode");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            nextState = States.Results;
            ExitState();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            nextState = States.Onboarding;
            ExitState();
        }

    }

}