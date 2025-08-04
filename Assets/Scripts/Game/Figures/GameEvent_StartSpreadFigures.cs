public readonly struct GameEvent_StartSpreadFigures
{
    public int FiguresCount { get; }

    public GameEvent_StartSpreadFigures(int figuresCount)
    {
        FiguresCount = figuresCount;
    }
}
