using System;

public struct GameEvent_OnFigureSpawned : IDisposable
{
    public GO_Figure Figure { get; private set; }

    public GameEvent_OnFigureSpawned(GO_Figure figure)
    {
        Figure = figure;
    }

    public void Dispose()
    {
        Figure = null;
    }
}