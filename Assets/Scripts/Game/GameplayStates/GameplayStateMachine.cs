using System;
using System.Collections.Generic;

public class GameplayStateMachine
{
    private readonly Dictionary<Type, IGameplayState> _states = new();
    private IGameplayState _currentState;

    public void Register<TState>(TState state) where TState : IGameplayState
    {
        _states[typeof(TState)] = state;
    }

    public void ChangeState<TState>() where TState : IGameplayState
    {
        _currentState?.Exit();

        _currentState = _states[typeof(TState)];
        _currentState.Enter();
    }

    public TState GetState<TState>() where TState : class, IGameplayState
    {
        return _states[typeof(TState)] as TState;
    }
}
