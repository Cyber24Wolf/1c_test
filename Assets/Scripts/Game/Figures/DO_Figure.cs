using UnityEngine;

[CreateAssetMenu(fileName = "FigureDataObject", menuName = "Game/FigureDataObject")]
public class DO_Figure : ScriptableObject
{
    public Sprite sprite;
    public Vector3 spawnOffset;
}
