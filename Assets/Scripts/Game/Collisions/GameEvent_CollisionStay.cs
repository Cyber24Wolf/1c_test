public readonly struct GameEvent_CollisionStay
{
    public GO_LightCollider A { get; }
    public GO_LightCollider B { get; }

    public GameEvent_CollisionStay(GO_LightCollider a, GO_LightCollider b)
    {
        A = a;
        B = b;
    }
}
