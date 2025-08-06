using System;
using Unity.VisualScripting;

public interface IFiguresDestroyer { }

public class FiguresDestroyer : IFiguresDestroyer, IDisposable
{
    private readonly EventBus   _eventBus;
    private readonly FigurePool _figurePool;

    public FiguresDestroyer(EventBus eventBus, FigurePool figurePool)
    {
        _eventBus   = eventBus;
        _figurePool = figurePool;

        _eventBus.Subscribe<GameEvent_FigureCollisionDetected>(OnDamageDetected);
        _eventBus.Subscribe<GameEvent_FigureSortingCorrect>(OnSortingCorrect);
        _eventBus.Subscribe<GameEvent_FigureSortingWrong>(OnSortingWrong);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_FigureCollisionDetected>(OnDamageDetected);
        _eventBus.Unsubscribe<GameEvent_FigureSortingCorrect>(OnSortingCorrect);
        _eventBus.Unsubscribe<GameEvent_FigureSortingWrong>(OnSortingWrong);
    }

    private void OnSortingCorrect(GameEvent_FigureSortingCorrect e)
    {
        HideFigure(e.Figure, explode: false);
    }

    private void OnSortingWrong(GameEvent_FigureSortingWrong e)
    {
        HideFigure(e.Figure, explode: true);
    }

    private void OnDamageDetected(GameEvent_FigureCollisionDetected e)
    {
        HideFigure(e.Figure, explode: false);
    }

    private void HideFigure(GO_Figure figure, bool explode)
    {
        figure.Model.HideCommand.Execute(new GO_Figure.HideInput(explode, OnFigureHide));
    }

    private void OnFigureHide(GO_Figure figure)
    {
        _figurePool.Despawn(figure);
        _eventBus.Publish(new GameEvent_OnFigureDestroyed(figure));
    }
}