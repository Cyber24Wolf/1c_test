using UnityEngine;
using Zenject;

public interface IFiguresFactory
{
    GO_Figure Spawn(DO_Figure figureData, Vector3 position, Vector2 velocity);
}

public class FiguresFactory : IFiguresFactory
{
    private readonly FigurePool _pool;

    public FiguresFactory(FigurePool squarePool)
    {
        _pool = squarePool;
    }

    public GO_Figure Spawn(DO_Figure data, Vector3 position, Vector2 velocity)
    {
        return _pool.Spawn(data, position, velocity);
    }
}

public class FigurePool : MonoMemoryPool<DO_Figure, Vector3, Vector2, GO_Figure>
{
    protected override void Reinitialize(DO_Figure data, Vector3 position, Vector2 velocity, GO_Figure item)
    {
        item.Model.FigureData.Value = data;
        item.transform.position = position;
        item.Model.Velocity.Value = velocity;
    }
}