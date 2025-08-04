using Zenject;

public interface ISorterSlotFactory
{
    GO_SorterSlot Spawn(DO_Figure figureData);
}

public class SorterSlotFactory : ISorterSlotFactory
{
    private readonly SorterSlotPool _pool;

    public SorterSlotFactory(SorterSlotPool pool)
    {
        _pool = pool;
    }

    public GO_SorterSlot Spawn(DO_Figure figureData)
    {
        return _pool.Spawn(figureData);
    }
}

public class SorterSlotPool : MonoMemoryPool<DO_Figure, GO_SorterSlot>
{
    protected override void Reinitialize(DO_Figure data, GO_SorterSlot item)
    {
        item.Model.FigureData.Value = data;
    }

    protected override void OnDespawned(GO_SorterSlot item)
    {
        item.Model.FigureData.Value = null;
    }
}
