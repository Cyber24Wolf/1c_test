public readonly struct GameEvent_FigureCollisionDetected
{
    public GO_Figure       Figure { get; }
    public GO_DamageDealer Dealer { get; }

    public GameEvent_FigureCollisionDetected(GO_Figure figure, GO_DamageDealer dealer)
    {
        Figure = figure;
        Dealer = dealer;
    }
}