using System;

public interface IGame
{
}

public class Game : IGame, IDisposable
{
    private readonly GameplayStateMachine _stateMachine;
    private readonly EventBus             _eventBus;
    private readonly ILifeService         _lifeService;

    public Game(EventBus eventBus, IGameplayConfig gameplayConfig, ILifeService lifeService, IScoresService scoresService)
    {
        _eventBus    = eventBus;
        _lifeService = lifeService;

        _stateMachine = new GameplayStateMachine();
        _stateMachine.Register(new GameplayState_Idle(_eventBus, gameplayConfig));
        _stateMachine.Register(new GameplayState_Playing(_eventBus, gameplayConfig));
        _stateMachine.Register(new GameplayState_Win(_eventBus, scoresService));
        _stateMachine.Register(new GameplayState_Loose(_eventBus, scoresService));

        _eventBus.Subscribe<GameEvent_OnDeath>(OnDeath);
        _eventBus.Subscribe<GameEvent_StartGameRequest>(OnGameStartEvent);
        _eventBus.Subscribe<GameEvent_NoFiguresLeft>(OnNoFiguresLeftEvent);

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

    private void OnNoFiguresLeftEvent(GameEvent_NoFiguresLeft e)
    {
        if (_stateMachine.GetCurrentState() is not GameplayState_Playing)
            return;

        if (_lifeService.IsAlive())
            Win();
        else
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
