using System;

public interface IScoresService 
{
    int GetCurrent();
}

public class ScoresService : IScoresService, IDisposable
{
    private readonly EventBus _eventBus;

    private int _scoresValue;

    public ScoresService(EventBus eventBus)
    {
        _eventBus = eventBus;

        _eventBus.Subscribe<GameEvent_SetScoresRequest>(OnSetScoresRequest);
        _eventBus.Subscribe<GameEvent_AddScoresRequest>(OnAddScoresRequest);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_SetScoresRequest>(OnSetScoresRequest);
        _eventBus.Unsubscribe<GameEvent_AddScoresRequest>(OnAddScoresRequest);
    }

    public int GetCurrent()
    {
        return _scoresValue;
    }

    private void OnSetScoresRequest(GameEvent_SetScoresRequest e)
    {
        _eventBus.Publish(new GameEvent_OnScoresSet(e.NewValue, _scoresValue));
        _scoresValue = e.NewValue;
    }

    private void OnAddScoresRequest(GameEvent_AddScoresRequest e) 
    {
        var oldValue = _scoresValue;
        _scoresValue += e.Value;
        _eventBus.Publish(new GameEvent_OnScoresAdd(oldValue, _scoresValue, e.Value));
    }
}
