using System;
using UnityEngine;

public interface ILifeService
{
}

public class LifeService : ILifeService, IDisposable
{
    private readonly EventBus _eventBus;

    private int _currentLifes;

    public LifeService(EventBus eventBus)
    {
        _eventBus = eventBus;

        _eventBus.Subscribe<GameEvent_DealDamageRequest>(OnDealDamageRequest);
        _eventBus.Subscribe<GameEvent_SetLifesRequest>(OnSetLifes);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_DealDamageRequest>(OnDealDamageRequest);
        _eventBus.Unsubscribe<GameEvent_SetLifesRequest>(OnSetLifes);
    }

    private void OnDealDamageRequest(GameEvent_DealDamageRequest e)
    {
        if (_currentLifes == 0)
            return;

        var oldLifes = _currentLifes;
        _currentLifes -= e.Damage;
        _currentLifes = Mathf.Max(0, _currentLifes);

        _eventBus.Publish(new GameEvent_OnDealDamage(oldLifes, _currentLifes, e.Damage));
        if (_currentLifes == 0)
            _eventBus.Publish(new GameEvent_OnDeath());
    }

    private void OnSetLifes(GameEvent_SetLifesRequest e) 
    {
        _eventBus.Publish(new GameEvent_OnSetLifes(_currentLifes, e.Value));
        _currentLifes = e.Value;
    }
}