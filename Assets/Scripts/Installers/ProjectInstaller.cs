using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<EventBus>()
            .AsSingle();
        Container
            .BindInterfacesTo<CollisionService>()
            .AsSingle();
        Container
            .BindInterfacesTo<PointerDragService>()
            .AsSingle();
        Container
            .BindInterfacesTo<InputService>()
            .AsSingle();
    }
}