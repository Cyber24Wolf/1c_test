public interface IGame
{
    public void Start();
    public void Restart();
}

public class GameLoop : IGame
{
    private readonly GameplayStateMachine _stateMachine;
    private readonly EventBus _eventBus;

    public GameLoop(EventBus eventBus)
    {
        _eventBus = eventBus;

        _stateMachine = new GameplayStateMachine();
        Initialize();
    }

    private void Initialize()
    {
        _stateMachine.Register(new GameplayState_Idle());
        _stateMachine.Register(new GameplayState_Playing());
        _stateMachine.Register(new GameplayState_Win());
        _stateMachine.Register(new GameplayState_Loose());

        _eventBus.Subscribe<GameEvent_LifesChanged>(OnLifesChanged);

        _stateMachine.ChangeState<GameplayState_Idle>();
    }

    public void Start()
    {
        _stateMachine.ChangeState<GameplayState_Playing>();
    }

    private void OnLifesChanged(GameEvent_LifesChanged e)
    {
        if (e.New <= 0)
            Loose();
    }

    public void Restart()
    {
        _stateMachine.ChangeState<GameplayState_Idle>();
    }

    private void Loose()
    {
        _stateMachine.ChangeState<GameplayState_Loose>();
    }
}
