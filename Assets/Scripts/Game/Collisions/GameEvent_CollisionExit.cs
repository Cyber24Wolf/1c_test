public readonly struct GameEvent_CollisionExit
{
    public GO_LightCollider A { get; }
    public GO_LightCollider B { get; }

    public GameEvent_CollisionExit(GO_LightCollider a, GO_LightCollider b)
    {
        A = a;
        B = b;
    }
}
