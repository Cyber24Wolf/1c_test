using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GO_Figure     _figuresPrefab;
    [SerializeField] private int           _initialFiguresCount = 3;
    [SerializeField] private GO_SorterSlot _sorterSlot;
    [SerializeField] private int           _initialSorterSlotsCount = 3;
    [SerializeField] private Camera       _camera;

    public override void InstallBindings()
    {
        Container
            .BindMemoryPool<GO_Figure, FigurePool>()
            .WithInitialSize(_initialFiguresCount)
            .FromComponentInNewPrefab(_figuresPrefab)
            .UnderTransformGroup("Figures");
        Container
            .Bind<IFiguresFactory>()
            .To<FiguresFactory>()
            .AsSingle();

        Container
            .BindMemoryPool<GO_SorterSlot, SorterSlotPool>()
            .WithInitialSize(_initialSorterSlotsCount)
            .FromComponentInNewPrefab(_sorterSlot)
            .UnderTransformGroup("SorterSlots");
        Container
            .Bind<ISorterSlotFactory>()
            .To<SorterSlotFactory>()
            .AsSingle();

        Container
            .Bind<Camera>()
            .FromInstance(_camera)
            .AsSingle();
    }
}