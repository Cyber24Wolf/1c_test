public readonly struct GameEvent_OnFiguresCounterChanged
{
    public int Alive       { get; }
    public int Destroyed   { get; }
    public int LeftToSpawn { get; }

    public GameEvent_OnFiguresCounterChanged(int alive, int destroyed, int leftToSpawn)
    {
        Alive       = alive;
        Destroyed   = destroyed;
        LeftToSpawn = leftToSpawn;
    }
}
