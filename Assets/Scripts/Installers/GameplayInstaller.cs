using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameplayConfig _gameplayConfig;

    public override void InstallBindings()
    {
        Container
            .Bind<IGameplayConfig>()
            .FromInstance(_gameplayConfig)
            .AsSingle();

        Container
            .Bind<IGame>()
            .To<Game>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<IDetectDamageService>()
            .To<DetectDamageService>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<IFiguresDestroyer>()
            .To<FiguresDestroyer>()
            .AsSingle()
            .NonLazy();
        Container
            .Bind<IFigureDragService>()
            .To<FiguresDragService>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<IFigureSortingLogic>()
            .To<FigureSortingLogic>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<ILifeService>()
            .To<LifeService>()
            .AsSingle()
            .NonLazy();
        Container
            .Bind<IScoresService>()
            .To<ScoresService>()
            .AsSingle()
            .NonLazy();
    }
}