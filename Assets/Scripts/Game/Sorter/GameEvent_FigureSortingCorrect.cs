using System;

public struct GameEvent_FigureSortingCorrect : IDisposable
{
    public GO_SorterSlot Slot   { get; private set; }
    public GO_Figure     Figure { get; private set; }

    public GameEvent_FigureSortingCorrect(GO_SorterSlot slot, GO_Figure figure)
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