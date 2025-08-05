public readonly struct GameEvent_InputEnabled
{
    public readonly InputSystem_Actions InputActions;
    public GameEvent_InputEnabled(InputSystem_Actions inputActions)
    {
        InputActions = inputActions;
    }
}