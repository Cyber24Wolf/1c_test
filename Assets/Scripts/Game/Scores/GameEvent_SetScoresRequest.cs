public readonly struct GameEvent_SetScoresRequest
{
    public int NewValue { get; }

    public GameEvent_SetScoresRequest(int newValue)
    {
        NewValue = newValue;
    }
}