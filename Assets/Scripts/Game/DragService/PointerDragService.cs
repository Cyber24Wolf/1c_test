using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IDragService
{
    public void Register  (GO_Draggable draggable);
    public void Unregister(GO_Draggable draggable);
}

public class PointerDragService : IDragService
{
    private readonly EventBus           _eventBus;
    private readonly List<GO_Draggable> _draggables = new();
    private Camera                      _mainCamera;

    private GO_Draggable        _current;
    private Vector3             _dragOffset;
    private InputSystem_Actions _inputActions;

    public PointerDragService(EventBus eventBus, Camera mainCamera)
    {
        _eventBus   = eventBus;
        _mainCamera = mainCamera;

        _eventBus.Subscribe<GameEvent_InputEnabled>(OnInputEnabled);
        _eventBus.Subscribe<GameEvent_InputDisabled>(OnInputDisabled);
    }

    public void Register(GO_Draggable draggable)
    {
        if (_draggables.Contains(draggable))
            return;
        _draggables.Add(draggable);
    }

    public void Unregister(GO_Draggable draggable)
    {
        if (!_draggables.Contains(draggable))
            return;
        _draggables.Remove(draggable);
    }

    private void OnInputEnabled(GameEvent_InputEnabled e)
    {
        _inputActions = e.InputActions;
        _inputActions.Player.PointerPosition.performed += Drag;
        _inputActions.Player.PointerPress.performed    += TryStartDrag;
        _inputActions.Player.PointerPress.canceled     += StopDrag;
    }

    private void OnInputDisabled(GameEvent_InputDisabled e)
    {
        _inputActions.Player.PointerPosition.performed -= Drag;
        _inputActions.Player.PointerPress.performed    -= TryStartDrag;
        _inputActions.Player.PointerPress.canceled     -= StopDrag;
        _inputActions = null;
        StopDrag();
    }

    private void TryStartDrag(InputAction.CallbackContext context)
    {
        if (_current != null)
            return;

        var mouseWorldPos = GetMouseWorldPosition(_inputActions.Player.PointerPosition.ReadValue<Vector2>());

        for (int i = _draggables.Count - 1; i >= 0; i--)
        {
            var draggable = _draggables[i];
            if (draggable == null)
                continue;

            if (draggable.TryGetComponent<GO_LightCollider>(out var collider) &&
                collider.ContainsPoint(mouseWorldPos))
            {
                _current = draggable;
                _dragOffset = draggable.transform.position - mouseWorldPos;
                _eventBus.Publish(new GameEvent_OnDragStart(_current));
                return;
            }
        }
    }

    private void StopDrag(InputAction.CallbackContext context)
    {
        StopDrag();
    }

    private void StopDrag()
    {
        if (_current == null)
            return;
        _eventBus.Publish(new GameEvent_OnDragStop(_current));
        _current = null;
    }

    private void Drag(InputAction.CallbackContext callbackContext)
    {
        if (_current == null)
            return;

        if (_current.enabled == false)
        {
            StopDrag();
            return;
        }

        Vector2 mouseWorldPos = GetMouseWorldPosition(callbackContext.ReadValue<Vector2>());
        _current.transform.position = mouseWorldPos + (Vector2)_dragOffset;
    }

    private Vector3 GetMouseWorldPosition(Vector3 mouseScreenPos)
    {
        mouseScreenPos.z = -_mainCamera.transform.position.z;
        return _mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
}
