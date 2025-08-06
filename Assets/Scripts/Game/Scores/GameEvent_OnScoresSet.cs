public readonly struct GameEvent_OnScoresSet
{
    public int OldValue { get; }
    public int NewValue { get; }

    public GameEvent_OnScoresSet(int oldValue, int newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}