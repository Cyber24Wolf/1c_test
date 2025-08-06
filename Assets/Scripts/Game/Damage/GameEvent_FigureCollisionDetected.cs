using System;

public struct GameEvent_FigureCollisionDetected : IDisposable
{
    public GO_Figure       Figure { get; private set; }
    public GO_DamageDealer Dealer { get; private set; }

    public GameEvent_FigureCollisionDetected(GO_Figure figure, GO_DamageDealer dealer)
    {
        Figure = figure;
        Dealer = dealer;
    }

    public void Dispose()
    {
        Figure = null;
        Dealer = null;
    }
}