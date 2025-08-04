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
    }
}