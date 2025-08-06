using System;

public struct GameEvent_CollisionStay : IDisposable
{
    public GO_LightCollider A { get; private set; }
    public GO_LightCollider B { get; private set; }

    public GameEvent_CollisionStay(GO_LightCollider a, GO_LightCollider b)
    {
        A = a;
        B = b;
    }

    public void Dispose()
    {
        A = null;
        B = null;
    }
}
