public class GameplayState_Loose : GameplayState_GameFinished
{
    public override bool IsWin => false;

    public GameplayState_Loose(EventBus eventBus) : base(eventBus)
    {
    }
}
