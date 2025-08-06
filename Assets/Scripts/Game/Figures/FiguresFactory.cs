using UnityEngine;
using Zenject;

public interface IFiguresFactory
{
    GO_Figure Spawn(DO_Figure figureData, Vector3 position, Vector2 velocity, CollisionLayerMask collisionLayerMask, bool manualControl);
}

public class FiguresFactory : IFiguresFactory
{
    private readonly FigurePool _pool;

    public FiguresFactory(FigurePool squarePool)
    {
        _pool = squarePool;
    }

    public GO_Figure Spawn(DO_Figure data, Vector3 position, Vector2 velocity, CollisionLayerMask collisionLayerMask, bool manualControl)
    {
        return _pool.Spawn(data, position, velocity, collisionLayerMask, manualControl);
    }
}

public class FigurePool : MonoMemoryPool<DO_Figure, Vector3, Vector2, CollisionLayerMask, bool, GO_Figure>
{
    protected override void Reinitialize(DO_Figure data, Vector3 position, Vector2 velocity, CollisionLayerMask collisionLayerMask, bool manualControl, GO_Figure item)
    {
        item.transform.position = position;
        
        item.Model.FigureData   .Value = data;
        item.Model.Velocity     .Value = velocity;
        item.Model.ManualControl.Value = manualControl;

        if (item.TryGetComponent<GO_LightCollider>(out var collider))
            collider.SetCollisionLayerMask(collisionLayerMask);

        item.gameObject.SetActive(true);
    }

    protected override void OnDespawned(GO_Figure item)
    {
        item.Model.FigureData.Value = null;
        item.gameObject.SetActive(false);
    }
}