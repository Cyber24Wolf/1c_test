using System;
using System.Collections.Generic;

public interface IDragFigureToSorterLogic { }

public class DragFigureToSorterLogic : IDragFigureToSorterLogic, IDisposable
{
    private readonly EventBus               _eventBus;
    private readonly ICollisionService      _collisionService;
    private readonly List<GO_LightCollider> _tempIntersections = new();

    public DragFigureToSorterLogic(
        EventBus eventBus,
        ICollisionService collisionService)
    {
        _eventBus         = eventBus;
        _collisionService = collisionService;

        _eventBus.Subscribe<GameEvent_OnDragStop>(OnDragStop);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_OnDragStop>(OnDragStop);
    }

    private void OnDragStop(GameEvent_OnDragStop e)
    {
        if (e.Draggable == null)
            return;

        if (!e.Draggable.TryGetComponent<GO_Figure>(out var figure))
            return;

        if (!figure.TryGetComponent<GO_LightCollider>(out var figureCollider))
            return;

        _tempIntersections.Clear();
        _collisionService.GetIntersections(figureCollider, _tempIntersections);

        foreach (var collider in _tempIntersections)
        {
            if (!collider.TryGetComponent<GO_SorterSlot>(out var slot))
                continue;
            if (slot.Model.FigureData.Value == null)
                continue;
            
            if (slot.Model.FigureData.Value == figure.Model.FigureData.Value)
                _eventBus.Publish(new GameEvent_FigureSortingCorrect(slot, figure));
            else
                _eventBus.Publish(new GameEvent_FigureSortingWrong(slot, figure));

        }
    }
}