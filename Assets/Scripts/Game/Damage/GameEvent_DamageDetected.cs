public readonly struct GameEvent_DamageDetected
{
    public GO_Figure       Figure { get; }
    public GO_DamageDealer Dealer { get; }
    public int             Damage { get; }

    public GameEvent_DamageDetected(GO_Figure figure, GO_DamageDealer dealer, int damage)
    {
        Figure = figure;
        Dealer = dealer;
        Damage = damage;
    }
}