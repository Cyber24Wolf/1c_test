using System;

public class GameplayState_Idle : IGameplayState, IDisposable
{
    private readonly EventBus        _eventBus;
    private          IGameplayConfig _gameplayConfig;

    public GameplayState_Idle(EventBus eventBus, IGameplayConfig gameplayConfig)
    {
        _eventBus       = eventBus;
        _gameplayConfig = gameplayConfig;
    }

    public void Dispose()
    {
        _gameplayConfig = null;
    }

    public void Enter()
    {
        _eventBus.PublishSticky(new GameEvent_SpawnSorterSlotsRequest(_gameplayConfig.FigureTypes));
    }

    public void Exit()
    {
    }
}
