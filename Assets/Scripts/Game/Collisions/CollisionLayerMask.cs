using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collision/LayerMask")]
public class CollisionLayerMask : ScriptableObject
{
    [SerializeField] private List<CollisionLayer> _layers = new();

    public bool Contains(CollisionLayer layer)
    {
        if (layer == null) return false;
        return (CalculateMask() & (1 << layer.LayerID)) != 0;
    }

    public int CalculateMask()
    {
        var mask = 0;
        foreach (var layer in _layers)
            mask |= (1 << layer.LayerID);
        return mask;
    }
}