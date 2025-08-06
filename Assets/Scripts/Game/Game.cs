using System;

public interface IGame
{
}

public class Game : IGame, IDisposable
{
    private readonly GameplayStateMachine _stateMachine;
    private readonly EventBus _eventBus;

    public Game(EventBus eventBus, IGameplayConfig gameplayConfig)
    {
        _eventBus = eventBus;

        _stateMachine = new GameplayStateMachine();
        _stateMachine.Register(new GameplayState_Idle());
        _stateMachine.Register(new GameplayState_Playing(_eventBus, gameplayConfig));
        _stateMachine.Register(new GameplayState_Win());
        _stateMachine.Register(new GameplayState_Loose());

        _eventBus.Subscribe<GameEvent_OnDeath>(OnDeath);
        _eventBus.Subscribe<GameEvent_StartGameRequest>(OnGameStartEvent);

        _stateMachine.ChangeState<GameplayState_Idle>();
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_OnDeath>(OnDeath);
        _eventBus.Unsubscribe<GameEvent_StartGameRequest>(OnGameStartEvent);
    }

    private void OnDeath(GameEvent_OnDeath e)
    {
        Loose();
    }

    private void OnGameStartEvent(GameEvent_StartGameRequest evt) 
    {
        _stateMachine.ChangeState<GameplayState_Playing>();
    }

    private void Loose()
    {
        _stateMachine.ChangeState<GameplayState_Loose>();
    }

    private void Win()
    {
        _stateMachine.ChangeState<GameplayState_Win>();
    }
}
