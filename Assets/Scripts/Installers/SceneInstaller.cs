using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private GO_Figure _figuresPrefab;

    public override void InstallBindings()
    {
        Container
            .BindMemoryPool<GO_Figure, FigurePool>()
            .WithInitialSize(10)
            .FromComponentInNewPrefab(_figuresPrefab)
            .UnderTransformGroup("Figures");

        Container
            .Bind<IFiguresFactory>()
            .To<FiguresFactory>()
            .AsSingle();
    }
}