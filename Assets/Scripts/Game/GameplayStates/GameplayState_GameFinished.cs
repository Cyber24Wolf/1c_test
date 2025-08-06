public abstract class GameplayState_GameFinished : IGameplayState
{
    private readonly EventBus _eventBus;
    private readonly IScoresService _coresService;

    public abstract bool IsWin { get; }

    public GameplayState_GameFinished(EventBus eventBus, IScoresService scoresService)
    {
        _eventBus = eventBus;
        _coresService = scoresService;
    }

    public virtual void Enter()
    {
        _eventBus.Publish(new GameEvent_GameFinished(IsWin, _coresService.GetCurrent()));
    }

    public virtual void Exit()
    {
    }
}