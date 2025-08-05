public readonly struct GameEvent_InputDisabled
{
    public readonly InputSystem_Actions InputActions;
    public GameEvent_InputDisabled(InputSystem_Actions inputActions)
    {
        InputActions = inputActions;
    }
}