using UnityEngine;

[CreateAssetMenu(fileName = "CollisionLayer", menuName = "Collision/Layer", order = 1)]
public class CollisionLayer : ScriptableObject
{
    [Range(1, 31)] public int LayerID;
}
