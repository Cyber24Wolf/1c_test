public class GameplayState_Win : GameplayState_GameFinished
{
    public override bool IsWin => true;

    public GameplayState_Win(EventBus eventBus) : base(eventBus)
    {
    }
}
