public readonly struct GameEvent_SetFiguresCounterValuesRequest
{
    public int Alive       { get; }
    public int Destroyed   { get; }
    public int LeftToSpawn { get; }

    public GameEvent_SetFiguresCounterValuesRequest(int alive, int destroyed, int leftToSpawn)
    {
        Alive       = alive;
        Destroyed   = destroyed;
        LeftToSpawn = leftToSpawn;
    }
}
