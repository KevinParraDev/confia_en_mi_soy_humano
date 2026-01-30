using UnityEngine;

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

        settingsManager?.Initialize();
        main?.Initialize();
        main.PlayActivated += OnPressPlay;
    }
    public void OnPressPlay()
    {
        nextState = States.Onboarding;
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
