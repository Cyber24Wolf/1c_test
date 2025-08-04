using System;

public class GameplayState_Playing : IGameplayState, IDisposable
{
    private readonly EventBus _eventBus;
    private readonly IGameplayConfig _gameplayConfig;

    public GameplayState_Playing(EventBus eventBus, IGameplayConfig gameplayConfig)
    {
        _eventBus = eventBus;
        _gameplayConfig = gameplayConfig;
    }

    public void Enter()
    {
        _eventBus.Publish(new GameEvent_StartSpreadFigures(_gameplayConfig.FiguresCount));
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
    }
}
