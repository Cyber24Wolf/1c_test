public readonly struct GameEvent_GameFinished
{
    public bool IsWin  { get; }
    public int  Scores { get; }

    public GameEvent_GameFinished(bool isWin, int scores)
    {
        IsWin  = isWin;
        Scores = scores;
    }
}