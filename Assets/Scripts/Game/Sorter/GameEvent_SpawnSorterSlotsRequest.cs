using System;
using System.Collections.Generic;

public readonly struct GameEvent_SpawnSorterSlotsRequest : IDisposable
{
    public DO_Figure[] Figures { get; }

    public GameEvent_SpawnSorterSlotsRequest(List<DO_Figure> figures)
    {
        Figures = new DO_Figure[figures.Count];
        for (var i = 0; i < figures.Count; i++)
            Figures[i] = figures[i];
    }

    public void Dispose()
    {
        for (var i = 0; i < Figures.Length; i++)
            Figures[i] = null;
    }
}
