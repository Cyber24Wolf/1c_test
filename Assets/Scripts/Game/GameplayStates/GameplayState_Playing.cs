using System;

public class GameplayState_Playing : IGameplayState, IDisposable
{
    private readonly EventBus _eventBus;
    private IGameplayConfig _gameplayConfig;

    public GameplayState_Playing(EventBus eventBus, IGameplayConfig gameplayConfig)
    {
        _eventBus = eventBus;
        _gameplayConfig = gameplayConfig;
    }

    public void Enter()
    {
        _eventBus.Publish(new GameEvent_StartSpreadFigures(_gameplayConfig.FiguresCount));
        _eventBus.Publish(new GameEvent_EnableInputRequest());
        _eventBus.Publish(new GameEvent_SetLifesRequest(_gameplayConfig.InitialLifes));
        _eventBus.Publish(new GameEvent_SetFiguresCounterValuesRequest(alive: 0, destroyed: 0, _gameplayConfig.FiguresCount));
        _eventBus.Publish(new GameEvent_SetScoresRequest(newValue: 0));
    }

    public void Exit()
    {
        _eventBus.Publish(new GameEvent_StopSpreadFigures());
        _eventBus.Publish(new GameEvent_HideAllFiguresRequest());
        _eventBus.Publish(new GameEvent_DisableInputRequest());
    }

    public void Dispose()
    {
        _gameplayConfig = null;
    }
}
