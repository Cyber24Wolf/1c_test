using System.Collections.Generic;
using UnityEngine;

public interface IGameplayConfig
{
    int InitialLifes         { get; }
    int LifesPerMissedFigure { get; }
    int LifesPerWrongFigure  { get; }
    int ScoresPerFigure      { get; }
    int FiguresCount         { get; }

    List<DO_Figure> FigureTypes         { get; }
    Vector2         FiguresVelocityMin  { get; }
    Vector2         FiguresVelocityMax  { get; }
    float           FiguresSpawnTimeMin { get; }
    float           FiguresSpawnTimeMax { get; }
    float           FigureReturnSpeed   { get; }

    CollisionLayerMask VelocityControlMask { get; }
    CollisionLayerMask DragControlMask     { get; }
}

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Game/GameplayConfig")]
public class GameplayConfig : ScriptableObject, IGameplayConfig
{
    [Header("Balance")]
    [SerializeField] private int initialLifes         = 100;
    [SerializeField] private int lifesPerMissedFigure = 1;
    [SerializeField] private int lifesPerWrongFigure  = 10;
    [SerializeField] private int scoresPerFigure      = 1;
    [SerializeField] private int figuresCount         = 350;

    [Header("Figures spawn")]
    [SerializeField] private List<DO_Figure> figureTypes         = new();
    [SerializeField] private Vector2         figuresVelocityMin  = Vector2.right;
    [SerializeField] private Vector2         figuresVelocityMax  = Vector2.right * 2;
    [SerializeField] private float           figuresSpawnTimeMin = 2;
    [SerializeField] private float           figuresSpawnTimeMax = 5;
    [SerializeField] private float           figureReturnSpeed    = 0.5f;

    [Header("Figures Collisions")]
    [SerializeField] private CollisionLayerMask velocityControlMask;
    [SerializeField] private CollisionLayerMask dragControlMask;

    public int InitialLifes         => initialLifes;
    public int LifesPerMissedFigure => lifesPerMissedFigure;
    public int LifesPerWrongFigure  => lifesPerWrongFigure;
    public int ScoresPerFigure      => scoresPerFigure;
    public int FiguresCount         => figuresCount;

    public List<DO_Figure> FigureTypes => figureTypes;
    public Vector2 FiguresVelocityMin  => figuresVelocityMin;
    public Vector2 FiguresVelocityMax  => figuresVelocityMax;
    public float FiguresSpawnTimeMin   => figuresSpawnTimeMin;
    public float FiguresSpawnTimeMax   => figuresSpawnTimeMax;
    public float FigureReturnSpeed     => figureReturnSpeed;

    public CollisionLayerMask VelocityControlMask => velocityControlMask;
    public CollisionLayerMask DragControlMask     => dragControlMask;
}
