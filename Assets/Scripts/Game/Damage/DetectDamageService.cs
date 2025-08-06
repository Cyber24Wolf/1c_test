using System;

public interface IDetectDamageService
{
}

public class DetectDamageService : IDetectDamageService, IDisposable
{
    private readonly EventBus        _eventBus;
    private readonly IGameplayConfig _gameplayConfig;

    public DetectDamageService(
        EventBus eventBus,
        IGameplayConfig gameplayConfig)
    {
        _eventBus       = eventBus;
        _gameplayConfig = gameplayConfig;

        _eventBus.Subscribe<GameEvent_CollisionEnter>(DetectDamageFromCollision);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_CollisionEnter>(DetectDamageFromCollision);
    }

    private void DetectDamageFromCollision(GameEvent_CollisionEnter collision)
    {
        var figure       = collision.A.gameObject.GetComponent<GO_Figure>();
        var damageDealer = collision.B.gameObject.GetComponent<GO_DamageDealer>();

        if (figure == null || damageDealer == null)
        {
            figure       = collision.B.gameObject.GetComponent<GO_Figure>();
            damageDealer = collision.A.gameObject.GetComponent<GO_DamageDealer>();
        }

        if (figure == null || damageDealer == null)
            return;

        _eventBus.Publish(new GameEvent_FigureCollisionDetected(figure, damageDealer));
        _eventBus.Publish(new GameEvent_DealDamageRequest(_gameplayConfig.LifesPerMissedFigure));
    }
}