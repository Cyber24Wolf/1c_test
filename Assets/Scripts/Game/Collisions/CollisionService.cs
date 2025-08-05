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
    private HashSet<ColliderPair> _previousCollisions = new();
    private HashSet<ColliderPair> _currentCollisions = new();

    public CollisionService(EventBus eventBus)        => _eventBus = eventBus;
    public void Register(GO_LightCollider collider)   => _colliders.Add(collider);
    public void Unregister(GO_LightCollider collider) => _colliders.Remove(collider);
    public void Tick()                                => CheckIntersections();

    private void CheckIntersections()
    {
        _currentCollisions.Clear();

        for (int i = 0; i < _colliders.Count; i++)
        {
            var a = _colliders[i];
            for (int j = i + 1; j < _colliders.Count; j++)
            {
                var b = _colliders[j];
                
                if (!a.Intersects(b))
                    continue;
                
                if (!a.CanCollideWith(b) && !b.CanCollideWith(a))
                    continue;

                var pair = new ColliderPair(a, b);
                _currentCollisions.Add(pair);

                if (_previousCollisions.Contains(pair))
                {
                    if (a.CallStayEvent && b.CallStayEvent)
                        _eventBus.Publish(new GameEvent_CollisionStay(a, b));
                }    
                else
                    _eventBus.Publish(new GameEvent_CollisionEnter(a, b));
            }
        }

        foreach (var pair in _previousCollisions)
        {
            if (_currentCollisions.Contains(pair))
                continue;
            _eventBus.Publish(new GameEvent_CollisionExit(pair.A, pair.B));
        }

        (_previousCollisions, _currentCollisions) = (_currentCollisions, _previousCollisions);
    }

    public void Dispose()
    {
        for(int i = 0; i < _colliders.Count; i++)
            _colliders[i] = null;
        _colliders.Clear();
    }
}

public readonly struct ColliderPair : IEquatable<ColliderPair>
{
    public readonly GO_LightCollider A;
    public readonly GO_LightCollider B;

    public ColliderPair(GO_LightCollider a, GO_LightCollider b)
    {
        if (a.GetInstanceID() < b.GetInstanceID())
        {
            A = a;
            B = b;
        }
        else
        {
            A = b;
            B = a;
        }
    }

    public bool Equals(ColliderPair other)
        => A == other.A && B == other.B;

    public override bool Equals(object obj)
        => obj is ColliderPair other && Equals(other);

    public override int GetHashCode()
        => (A, B).GetHashCode();
}