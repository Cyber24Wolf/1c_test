using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public interface IFigureDragService{ }

public class FiguresDragService : IFigureDragService, IDisposable
{
    private readonly EventBus                 _eventBus;
    private readonly IGameplayConfig          _gameplayConfig;
    private readonly ICollisionService        _collisionService;
    private readonly Dictionary<int, Vector3> _lastPositions = new();

    public FiguresDragService(
        EventBus eventBus,
        IGameplayConfig gameplayConfig,
        ICollisionService collisionService)
    {
        _eventBus         = eventBus;
        _gameplayConfig   = gameplayConfig;
        _collisionService = collisionService;

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
        _lastPositions[figure.GetInstanceID()] = figure.transform.position;

        if (!figure.TryGetComponent<GO_LightCollider>(out var collider))
            return;
        collider.SetCollisionLayerMask(_gameplayConfig.DragControlMask);
    }

    private void OnDragStop(GameEvent_OnDragStop e)
    {
        if (e.Draggable == null)
            return;

        if (!e.Draggable.TryGetComponent<GO_Figure>(out var figure))
            return;

        if (figure.TryGetComponent<GO_LightCollider>(out var figureCollider))
        {
            CheckSorterSlots(figure, figureCollider, e.Draggable);
            return;
        }
        ReturnToLastPositionAsync(figure, e.Draggable).Forget();
    }

    private void CheckSorterSlots(GO_Figure figure, GO_LightCollider figureCollider, GO_Draggable draggable)
    {
        if (figureCollider == null)
            return;

        var result = new List<GO_LightCollider>();
        _collisionService.GetIntersections(figureCollider, result);

        GO_SorterSlot closestSort = null;
        float closestDistance = -1f;
        foreach (var collider in result)
        {
            if (!collider.TryGetComponent<GO_SorterSlot>(out var slot))
                continue;
            if (slot.Model.FigureData.Value == null)
                continue;

            var currentSqrDistance = (figure.transform.position - slot.transform.position).sqrMagnitude;
            if (closestDistance < 0 || ((closestDistance * closestDistance) > currentSqrDistance))
            {
                closestSort = slot;
                closestDistance = currentSqrDistance;
            }
        }

        if (closestSort == null)
        {
            ReturnToLastPositionAsync(figure, draggable).Forget();
            return;
        }

        if (closestSort.Model.FigureData.Value == figure.Model.FigureData.Value)
            _eventBus.Publish(new GameEvent_FigureSortingCorrect(closestSort, figure));
        else
            _eventBus.Publish(new GameEvent_FigureSortingWrong(closestSort, figure));
    }

    private async UniTask ReturnToLastPositionAsync(GO_Figure figure, GO_Draggable draggable)
    {
        var instanceId = figure.GetInstanceID();
        var pos = _lastPositions[instanceId];
        _lastPositions.Remove(instanceId);

        var time = (pos - figure.transform.position).magnitude / _gameplayConfig.FigureReturnSpeed;
        draggable.enabled = false;
        await LMotion
            .Create(figure.transform.position, pos, time)
            .WithEase(Ease.InSine)
            .BindToPosition(figure.transform)
            .ToAwaitable();
        figure.Model.ManualControl.Value = false;
        draggable.enabled = true;

        if (!figure.TryGetComponent<GO_LightCollider>(out var collider))
            return;
        collider.SetCollisionLayerMask(_gameplayConfig.VelocityControlMask);
    }
}