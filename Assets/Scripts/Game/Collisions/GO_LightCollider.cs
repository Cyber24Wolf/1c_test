using UnityEngine;
using Zenject;

public class GO_LightCollider : MonoBehaviour
{
    [SerializeField] private float              _radius        = 0.5f;
    [SerializeField] private bool               _callStayEvent = false;
    [SerializeField] private CollisionLayer     _layer;
    [SerializeField] private CollisionLayerMask _collisionMask;

#if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField] private Color _gizmoColor = Color.cyan;
#endif

    private ICollisionService _collisionService;

    public Vector2              Center        => transform.position;
    public float                Radius        => _radius;
    public bool                 CallStayEvent => _callStayEvent;

    [Inject]
    private void Setup(ICollisionService collisionService) 
        => _collisionService = collisionService;

    private void OnEnable()
    {
        if (_collisionService == null)
            return;

        _collisionService.Register(this);
    }

    private void OnDisable()
    {
        if (_collisionService == null)
            return;

        _collisionService.Unregister(this);
    }

    public bool ContainsPoint(Vector2 point)
    {
        return (point - Center).sqrMagnitude <= _radius * _radius;
    }

    public bool Intersects(GO_LightCollider other)
        => Intersects(other.Center, other.Radius);

    public bool Intersects(Vector2 otherCenter, float otherRadius)
    {
        float totalRadius = _radius + otherRadius;
        return (otherCenter - Center).sqrMagnitude <= totalRadius * totalRadius;
    }

    public bool CanCollideWith(GO_LightCollider other)
    {
        if (enabled == false || other.enabled == false)
            return false;

        if (gameObject.activeInHierarchy == false || other.gameObject.activeInHierarchy == false) 
            return false;

        if (_layer == null || other._layer == null)
            return true;

        return (_collisionMask.CalculateMask() & (1 << other._layer.LayerID)) != 0 &&
               (other._collisionMask.CalculateMask() & (1 << _layer.LayerID)) != 0;
    }

    public void SetCollisionLayerMask(CollisionLayerMask mask) => _collisionMask = mask;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(Center, _radius);
    }
#endif

}
