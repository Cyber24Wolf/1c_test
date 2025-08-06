using System;

public struct GameEvent_FigureSortingWrong : IDisposable
{
    public GO_SorterSlot Slot   { get; private set; }
    public GO_Figure     Figure { get; private set; }

    public GameEvent_FigureSortingWrong(GO_SorterSlot slot, GO_Figure figure)
    {
        Slot   = slot;
        Figure = figure;
    }

    public void Dispose()
    {
        Slot   = null;
        Figure = null;
    }
}