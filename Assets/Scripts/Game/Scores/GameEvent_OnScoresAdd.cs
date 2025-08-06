public readonly struct GameEvent_OnScoresAdd
{
    public int OldValue { get; }
    public int NewValue { get; }
    public int AddValue { get; }

    public GameEvent_OnScoresAdd(int oldValue, int newValue, int addedValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
        AddValue = addedValue;
    }
}