public readonly struct GameEvent_CollisionEnter
{
    public GO_LightCollider A { get; }
    public GO_LightCollider B { get; }

    public GameEvent_CollisionEnter(GO_LightCollider a, GO_LightCollider b)
    {
        A = a;
        B = b;
    }
}
