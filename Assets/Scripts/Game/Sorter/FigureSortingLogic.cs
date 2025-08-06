using System;

public interface IFigureSortingLogic
{
}

public class FigureSortingLogic : IFigureSortingLogic, IDisposable
{
    private readonly EventBus _eventBus;
    private IGameplayConfig _gameplayConfig;

    public FigureSortingLogic(EventBus eventBus, IGameplayConfig gameplayConfig)
    {
        _eventBus = eventBus;
        _gameplayConfig = gameplayConfig;
        
        _eventBus.Subscribe<GameEvent_FigureSortingCorrect>(OnSortingCorrect);
        _eventBus.Subscribe<GameEvent_FigureSortingWrong>(OnSortingWrong);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_FigureSortingCorrect>(OnSortingCorrect);
        _eventBus.Unsubscribe<GameEvent_FigureSortingWrong>(OnSortingWrong);
        _gameplayConfig = null;
    }

    private void OnSortingCorrect(GameEvent_FigureSortingCorrect e)
    {
        _eventBus.Publish(new GameEvent_AddScoresRequest(_gameplayConfig.ScoresPerFigure));
    }

    private void OnSortingWrong(GameEvent_FigureSortingWrong e)
    {
        _eventBus.Publish(new GameEvent_DealDamageRequest(_gameplayConfig.LifesPerWrongFigure));
    }

}