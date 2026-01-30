using UnityEngine;

public class ResultsState : GameStateBase
{

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
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    //TODO:DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            nextState = States.Gameplay;
            ExitState();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            nextState = States.Main;
            ExitState();
        }
    }
}
