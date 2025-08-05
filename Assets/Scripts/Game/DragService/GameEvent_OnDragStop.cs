public readonly struct GameEvent_OnDragStop
{
    public GO_Draggable Draggable { get; }

    public GameEvent_OnDragStop(GO_Draggable draggable)
    {
        Draggable = draggable;
    }
}