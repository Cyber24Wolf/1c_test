public interface ILifeService
{
}

public class LifeService : ILifeService
{
    private readonly EventBus _eventBus;

    public LifeService(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
}