using System;

public struct GameEvent_OnDragStop : IDisposable
{
    public GO_Draggable Draggable { get; private set; }

    public GameEvent_OnDragStop(GO_Draggable draggable)
    {
        Draggable = draggable;
    }

    public void Dispose()
    {
        Draggable = null;
    }
}