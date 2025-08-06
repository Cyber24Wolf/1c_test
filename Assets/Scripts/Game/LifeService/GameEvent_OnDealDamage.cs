public readonly struct GameEvent_OnDealDamage
{
    public int OldValue { get; }
    public int NewValue { get; }
    public int Damage   { get; }

    public GameEvent_OnDealDamage(int oldValue, int newValue, int damage)
    {
        OldValue = oldValue; 
        NewValue = newValue; 
        Damage = damage;
    }
}