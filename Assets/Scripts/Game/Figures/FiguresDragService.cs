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
    private readonly Dictionary<int, Vector3> _lastPositions = new();

    public FiguresDragService(
        EventBus eventBus,
        IGameplayConfig gameplayConfig)
    {
        _eventBus       = eventBus;
        _gameplayConfig = gameplayConfig;

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
        if (!e.Draggable.TryGetComponent<GO_Figure>(out var figure))
            return;

        ReturnToLastPositionAsync(figure).Forget();
    }

    private async UniTask ReturnToLastPositionAsync(GO_Figure figure)
    {
        var instanceId = figure.GetInstanceID();
        var pos = _lastPositions[instanceId];
        _lastPositions.Remove(instanceId);

        var time = (pos - figure.transform.position).magnitude / _gameplayConfig.FigureReturnSpeed;
        
        await LMotion
            .Create(figure.transform.position, pos, time)
            .WithEase(Ease.InSine)
            .BindToPosition(figure.transform)
            .ToAwaitable();
        figure.Model.ManualControl.Value = false;

        if (!figure.TryGetComponent<GO_LightCollider>(out var collider))
            return;
        collider.SetCollisionLayerMask(_gameplayConfig.VelocityControlMask);
    }
}