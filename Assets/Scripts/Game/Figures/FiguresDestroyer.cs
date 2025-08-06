public interface IFiguresDestroyer { }

public class FiguresDestroyer : IFiguresDestroyer
{
    private readonly FigurePool _figurePool;

    public FiguresDestroyer(
        EventBus   eventBus,
        FigurePool figurePool)
    {
        _figurePool = figurePool;
        eventBus.Subscribe<GameEvent_FigureCollisionDetected>(OnDamageDetected);
    }

    private void OnDamageDetected(GameEvent_FigureCollisionDetected e)
    {
        e.Figure.Model.HideCommand.Execute(new GO_Figure.HideInput(explode: false, OnFigureHide));
    }

    private void OnFigureHide(GO_Figure figure)
    {
        _figurePool.Despawn(figure);
    }
}