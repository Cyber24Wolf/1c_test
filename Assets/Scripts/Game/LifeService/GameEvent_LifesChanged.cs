public readonly struct GameEvent_LifesChanged
{
    public int Old { get; }
    public int New { get; }

    public GameEvent_LifesChanged(int oldLifes, int newLifes)
    {
        Old = oldLifes;
        New = newLifes;
    }
}