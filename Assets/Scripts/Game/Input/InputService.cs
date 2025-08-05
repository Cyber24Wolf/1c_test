using System;

public interface IInputService { }

public class InputService : IInputService, IDisposable
{
    private readonly EventBus _eventBus;

    private InputSystem_Actions _inputActions = new();

    public InputService(EventBus eventBus)
    {
        _eventBus = eventBus;

        eventBus.Subscribe<GameEvent_EnableInputRequest>(ProcessEnableRequest);
        eventBus.Subscribe<GameEvent_DisableInputRequest>(ProcessDisableRequest);        
    }

    private void ProcessEnableRequest(GameEvent_EnableInputRequest request)
    {
        _inputActions.Enable();
        _eventBus.Publish(new GameEvent_InputEnabled(_inputActions));
    }

    private void ProcessDisableRequest(GameEvent_DisableInputRequest request)
    {
        _inputActions.Disable();
        _eventBus.Publish(new GameEvent_InputDisabled(_inputActions));
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GameEvent_EnableInputRequest>(ProcessEnableRequest);
        _eventBus.Unsubscribe<GameEvent_DisableInputRequest>(ProcessDisableRequest);

        _inputActions.Disable();
        _inputActions.Dispose();
    }
}
