using System;

public struct GameEvent_OnFigureDestroyed : IDisposable
{
    public GO_Figure Figure { get; private set; }

    public GameEvent_OnFigureDestroyed(GO_Figure figure)
    {
        Figure = figure;
    }

    public void Dispose()
    {
        Figure = null;
    }
}