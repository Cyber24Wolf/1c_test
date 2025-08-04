public readonly struct GameEvent_Collision
{
    public GO_LightCollider A { get; }
    public GO_LightCollider B { get; }

    public GameEvent_Collision(GO_LightCollider a, GO_LightCollider b)
    {
        A = a;
        B = b;
    }
}
