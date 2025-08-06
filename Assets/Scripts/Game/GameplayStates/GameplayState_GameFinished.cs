public abstract class GameplayState_GameFinished : IGameplayState
{
    private readonly EventBus _eventBus;

    public abstract bool IsWin { get; }

    public GameplayState_GameFinished(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public virtual void Enter()
    {
        _eventBus.Publish(new GameEvent_GameFinished(IsWin));
    }

    public virtual void Exit()
    {
    }
}