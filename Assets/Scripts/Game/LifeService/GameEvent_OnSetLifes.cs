public readonly struct GameEvent_OnSetLifes
{
    public int OldValue { get; }
    public int NewValue { get; }

    public GameEvent_OnSetLifes(int oldValue, int newValue) 
    { 
        OldValue = oldValue;
        NewValue = newValue;
    }
}