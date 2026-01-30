using System;
using UnityEngine;

public abstract class GameStateBase : MonoBehaviour
{
    public Action<States> FinishState;
    protected States nextState;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        if (Globals.SceneDebugModeActivated)
            EnterState();
        else
            GameManager.GetState?.Invoke(this);
    }

    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {
        FinishState?.Invoke(nextState);
    }
}
