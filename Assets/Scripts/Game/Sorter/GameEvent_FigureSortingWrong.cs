public readonly struct GameEvent_FigureSortingWrong
{
    public GO_SorterSlot Slot   { get; }
    public GO_Figure     Figure { get; }

    public GameEvent_FigureSortingWrong(GO_SorterSlot slot, GO_Figure figure)
    {
        Slot   = slot;
        Figure = figure;
    }
}