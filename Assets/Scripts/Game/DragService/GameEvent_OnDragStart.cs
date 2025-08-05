public readonly struct GameEvent_OnDragStart
{
    public GO_Draggable Draggable { get; }

    public GameEvent_OnDragStart(GO_Draggable draggable)
    {
        Draggable = draggable;
    }
}