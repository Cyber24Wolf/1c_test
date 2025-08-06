public readonly struct GameEvent_GameFinished
{
    public bool IsWin { get; }

    public GameEvent_GameFinished(bool isWin)
    {
        IsWin = isWin;
    }
}