using System;
using UnityEngine;

public interface IFiguresCounter { }

public class FiguresCounter : IFiguresCounter, IDisposable
{
    private readonly EventBus _eventBus;

    private int _initialFiguresCount;
    private int _aliveFiguresCount;
    private int _destroyedFiguresCount;
    private int _leftToSpawnFiguresCount;

    public FiguresCounter(EventBus eventBus)
    {
        _eventBus = eventBus;

        _eventBus.Subscribe<GameEvent_OnFigureSpawned>(OnFigureSpawned);
        _eventBus.Subscribe<GameEvent_OnFigureDestroyed>(OnFigureDestroyed);
        _eventBus.Subscribe<GameEvent_SetFiguresCounterValuesRequest>(OnSetFiguresCounterRequest);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_OnFigureSpawned>(OnFigureSpawned);
        _eventBus.Unsubscribe<GameEvent_OnFigureDestroyed>(OnFigureDestroyed);
        _eventBus.Unsubscribe<GameEvent_SetFiguresCounterValuesRequest>(OnSetFiguresCounterRequest);
    }

    private void OnFigureSpawned(GameEvent_OnFigureSpawned e)
    {
        _aliveFiguresCount++;
        _leftToSpawnFiguresCount = Mathf.Max(0, _leftToSpawnFiguresCount - 1);
        PublishStateChanged();
    }

    private void OnFigureDestroyed(GameEvent_OnFigureDestroyed e)
    {
        _destroyedFiguresCount++;
        _aliveFiguresCount = Mathf.Max(0, _aliveFiguresCount - 1);
        PublishStateChanged();

        if (_destroyedFiguresCount == _initialFiguresCount && _destroyedFiguresCount != 0 && _aliveFiguresCount == 0)
            _eventBus.Publish(new GameEvent_NoFiguresLeft());
    }

    private void OnSetFiguresCounterRequest(GameEvent_SetFiguresCounterValuesRequest e)
    {
        _aliveFiguresCount       = e.Alive;
        _destroyedFiguresCount   = e.Destroyed;
        _leftToSpawnFiguresCount = e.LeftToSpawn;
        _initialFiguresCount     = e.LeftToSpawn;
        PublishStateChanged();
    }

    private void PublishStateChanged()
    {
        _eventBus.Publish(new GameEvent_OnFiguresCounterChanged(
            _aliveFiguresCount,
            _destroyedFiguresCount,
            _leftToSpawnFiguresCount));
    }
}
