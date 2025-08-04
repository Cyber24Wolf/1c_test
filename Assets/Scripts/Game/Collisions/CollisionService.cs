using System;
using System.Collections.Generic;
using Zenject;

public interface ICollisionService
{
    void Register(GO_LightCollider collider);
    void Unregister(GO_LightCollider collider);
}

public class CollisionService : ICollisionService, ITickable, IDisposable
{
    private EventBus _eventBus;
    private List<GO_LightCollider> _colliders = new();

    public CollisionService(EventBus eventBus)        => _eventBus = eventBus;
    public void Register(GO_LightCollider collider)   => _colliders.Add(collider);
    public void Unregister(GO_LightCollider collider) => _colliders.Remove(collider);
    public void Tick()                                => CheckIntersections();

    private void CheckIntersections()
    {
        if (_colliders.Count == 0)
            return;

        for (int i = 0; i < _colliders.Count; i++)
        {
            var a = _colliders[i];
            for (int j = i + 1; j < _colliders.Count; j++)
            {
                var b = _colliders[j];
                if (a.Intersects(b))
                    _eventBus.Publish(new GameEvent_Collision(a, b));
            }
        }
    }

    public void Dispose()
    {
        for(int i = 0; i < _colliders.Count; i++)
            _colliders[i] = null;
        _colliders.Clear();
    }
}