using System;

public struct GameEvent_OnDragStart : IDisposable
{
    public GO_Draggable Draggable { get; private set; }

    public GameEvent_OnDragStart(GO_Draggable draggable)
    {
        Draggable = draggable;
    }

    public void Dispose()
    {
        Draggable = null;
    }
}