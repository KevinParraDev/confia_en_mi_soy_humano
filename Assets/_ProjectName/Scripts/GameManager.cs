using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<GameStateBase> GetState;

    [SerializeField] private SceneLoaderController loadingController;
    [SerializeField] private States currentState;
    [SerializeField] private int fps = 60;

    private GameStateBase currentGameState;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = fps;
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        AddListeners();
        loadingController.LoadScene((int)currentState);
    }

    private void AddListeners()
    {
        loadingController.SwitchScenesCompleted += OnSceneChangeComplete;
        GameManager.GetState += OnGetState;
    }

    private void OnGetState(GameStateBase state)
    {
        Debug.Log(state.name);
        currentGameState = state;
        currentGameState.FinishState += OnChangeState;

        DependencyInjection(state);
    }

    private void DependencyInjection(GameStateBase state)
    {
        switch (state)
        {
            case MainState mainState:
                mainState.Dependencies();
                break;
            case OnboardingState onboardingState:
                onboardingState.Dependencies();
                break;
            case GameplayState gameplayState:
                gameplayState.Dependencies();
                break;
            case ResultsState resultsState:
                resultsState.Dependencies();
                break;
            case CamiloTestState camiloTestState:
                camiloTestState.Dependencies();
                break;
            case KevinTestState kevinTestState:
                kevinTestState.Dependencies();
                break;
            default:
                break;
        }
    }

    private void OnChangeState(States nextState)
    {
        Debug.Log(nextState.ToString());
        currentGameState.FinishState -= OnChangeState;

        loadingController.SwitchScenes((int)currentState, (int)nextState);

        currentState = nextState;
    }

    private void OnSceneChangeComplete()
    {
        if (currentGameState != null)
        {
            currentGameState.EnterState();
        }
    }

    private void RemoveListeners()
    {
        if (currentGameState != null)
            currentGameState.FinishState -= OnChangeState;

        GameManager.GetState -= OnGetState;
        loadingController.SwitchScenesCompleted -= OnSceneChangeComplete;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            
        }
        else
        {
            
        }
    }

    private void OnApplicationQuit()
    {
        RemoveListeners();
        if(currentGameState != null)
            currentGameState.ExitState();
    }
}
