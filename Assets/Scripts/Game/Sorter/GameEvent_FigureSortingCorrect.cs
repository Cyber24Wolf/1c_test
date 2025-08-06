public readonly struct GameEvent_FigureSortingCorrect
{
    public GO_SorterSlot Slot   { get; }
    public GO_Figure     Figure { get; }

    public GameEvent_FigureSortingCorrect(GO_SorterSlot slot, GO_Figure figure)
    {
        Slot   = slot;
        Figure = figure;
    }
}