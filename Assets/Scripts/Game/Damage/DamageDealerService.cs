public interface IDamageDealerService
{
}

public class DamageDealerService : IDamageDealerService
{
    private readonly EventBus        _eventBus;
    private readonly IGameplayConfig _gameplayConfig;

    public DamageDealerService(
        EventBus eventBus,
        IGameplayConfig gameplayConfig)
    {
        _eventBus       = eventBus;
        _gameplayConfig = gameplayConfig;

        _eventBus.Subscribe<GameEvent_CollisionEnter>(DetectDamageFromCollision);
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

        _eventBus.Publish(new GameEvent_DamageDetected(figure, damageDealer, _gameplayConfig.LifesPerFigure));
    }
}