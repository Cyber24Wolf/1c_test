using System;
using System.Collections.Generic;

public interface IFiguresDestroyer 
{
    public void Register(GO_Figure figure);
    public void Unregister(GO_Figure figure);
}

public class FiguresDestroyer : IFiguresDestroyer, IDisposable
{
    private readonly EventBus   _eventBus;
    private readonly FigurePool _figurePool;

    private Dictionary<int, GO_Figure> _hidingFigures = new();
    private List<GO_Figure>            _activeFigures = new();

    public FiguresDestroyer(EventBus eventBus, FigurePool figurePool)
    {
        _eventBus   = eventBus;
        _figurePool = figurePool;

        _eventBus.Subscribe<GameEvent_FigureCollisionDetected>(OnDamageDetected);
        _eventBus.Subscribe<GameEvent_FigureSortingCorrect>(OnSortingCorrect);
        _eventBus.Subscribe<GameEvent_FigureSortingWrong>(OnSortingWrong);
        _eventBus.Subscribe<GameEvent_HideAllFiguresRequest>(OnHideAllFiguresRequest);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_FigureCollisionDetected>(OnDamageDetected);
        _eventBus.Unsubscribe<GameEvent_FigureSortingCorrect>(OnSortingCorrect);
        _eventBus.Unsubscribe<GameEvent_FigureSortingWrong>(OnSortingWrong);
        _eventBus.Unsubscribe<GameEvent_HideAllFiguresRequest>(OnHideAllFiguresRequest);
    }

    public void Register(GO_Figure figure)
    {
        if (_activeFigures.Contains(figure))
            return;

        _activeFigures.Add(figure);
    }

    public  void Unregister(GO_Figure figure)
    {
        if (!_activeFigures.Contains(figure))
            return;

        _activeFigures.Remove(figure);
    }

    private void OnDamageDetected(GameEvent_FigureCollisionDetected e)
    {
        HideFigure(e.Figure, explode: false);
    }

    private void OnSortingCorrect(GameEvent_FigureSortingCorrect e)
    {
        HideFigure(e.Figure, explode: false);
    }

    private void OnSortingWrong(GameEvent_FigureSortingWrong e)
    {
        HideFigure(e.Figure, explode: true);
    }

    private void OnHideAllFiguresRequest(GameEvent_HideAllFiguresRequest e)
    {
        for (int i = 0; i < _activeFigures.Count; i++)
            HideFigure(_activeFigures[i], explode: false);
    }

    private void HideFigure(GO_Figure figure, bool explode)
    {
        var figureId = figure.GetInstanceID();
        if (_hidingFigures.ContainsKey(figureId))
            return;

        _hidingFigures[figureId] = figure;
        figure.Model.HideCommand.Execute(new GO_Figure.HideInput(explode, OnFigureHide));
    }

    private void OnFigureHide(GO_Figure figure)
    {
        _figurePool.Despawn(figure);
        _eventBus.Publish(new GameEvent_OnFigureDestroyed(figure));

        var figureId = figure.GetInstanceID();
        if (!_hidingFigures.ContainsKey(figureId))
            return;
        _hidingFigures.Remove(figureId);
    }
}