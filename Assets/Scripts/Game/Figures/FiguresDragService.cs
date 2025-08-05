using System;

public interface IFigureDragService{ }

public class FiguresDragService : IFigureDragService, IDisposable
{
    private readonly EventBus _eventBus;

    public FiguresDragService(EventBus eventBus)
    {
        _eventBus = eventBus;

        eventBus.Subscribe<GameEvent_OnDragStart>(OnDragStart);
        eventBus.Subscribe<GameEvent_OnDragStop>(OnDragStop);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_OnDragStart>(OnDragStart);
        _eventBus.Unsubscribe<GameEvent_OnDragStop>(OnDragStop);
    }

    private void OnDragStart(GameEvent_OnDragStart e)
    {
        if (!e.Draggable.TryGetComponent<GO_Figure>(out var figure))
            return;

        figure.Model.ManualControl.Value = true;
    }

    private void OnDragStop(GameEvent_OnDragStop e)
    {
        if (!e.Draggable.TryGetComponent<GO_Figure>(out var figure))
            return;

        figure.Model.ManualControl.Value = true;
    }
}