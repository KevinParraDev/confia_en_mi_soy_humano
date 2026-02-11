using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;
    private Dictionary<NPCState, IState> states = new Dictionary<NPCState, IState>();

    private bool isInitialized = false;

    public void Initialize()
    {
        isInitialized = true;
    }

    public void AddState(NPCState stateType, IState state)
    {
        states[stateType] = state;
    }

    public void ChangeState(NPCState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = states[newState];
        if(currentState == null)
        {
            Debug.LogError($"StateMachine: State {newState} not found in states dictionary.");
            return;
        }
        currentState.Enter();
    }

    void Update()
    {
        if (!isInitialized)
            return;

        if (currentState != null)
        {
            currentState.Execute();
        }
        else
        {
            Debug.LogWarning("StateMachine has no current state set.");
        }
    }

    public NPCState GetCurrentStateType()
    {
        foreach (var pair in states)
        {
            if (pair.Value == currentState)
            {
                return pair.Key;
            }
        }
        Debug.LogWarning("Current state not found in states dictionary.");
        return default;
    }

    public IState GetCurrentState()
    {
        return currentState;
    }
}

public enum NPCState
{
    Idle,
    Patrol,
    Chase,
    Interact,
    Panic,
    Stun,
    InBath,
    BackToIdle,
    GoToDialogue
}
