public readonly struct GameEvent_DealDamageRequest
{
    public int Damage { get; }
    public GameEvent_DealDamageRequest(int damage)
    {
        Damage = damage;
    }
}