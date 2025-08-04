using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GO_Sorter : MonoBehaviour
{
    [SerializeField] float _distanceBetweenSlots = 1.5f;
    [SerializeField] Vector2 _backgroundPadding = Vector2.one;
    [SerializeField] float _backgroundHeight = 0.5f;
    [SerializeField] SpriteRenderer _background;

    private EventBus            _eventBus;
    private ISorterSlotFactory  _sorterSlotFactory;
    private List<GO_SorterSlot> _slots = new();

    [Inject]
    private void Setup(
        EventBus eventBus, 
        ISorterSlotFactory sorterSlotFactory)
    {
        _eventBus = eventBus;
        _sorterSlotFactory = sorterSlotFactory;

        _eventBus.Subscribe<GameEvent_SpawnSorterSlotsRequest>(OnSpawnRequest);
    }

    private void OnSpawnRequest(GameEvent_SpawnSorterSlotsRequest request)
    {
        _slots.Clear();
        for (var i = 0; i < request.Figures.Length; i++)
            _slots.Add(
                _sorterSlotFactory.Spawn(request.Figures[i])
            );
        UpdateSlotsPositions();
    }

    private void UpdateSlotsPositions()
    {
        if (_slots == null && _slots.Count == 0)
            return;

        var middle = _slots.Count / 2;
        for (var i = 0; i < _slots.Count; i++)
        {
            var offset = i - middle;
            var adjust = (_slots.Count % 2 == 0) ? _distanceBetweenSlots / 2 : 0;
            var pos = transform.position + _slots[i].Model.FigureData.Value.spawnOffset + transform.right * (offset * _distanceBetweenSlots + adjust);
            _slots[i].transform.position = pos;
        }

        ResizeBackground(_slots.Count);
    }

    private void ResizeBackground(int count)
    {
        var width = (count - 1) * _distanceBetweenSlots + _backgroundPadding.x * 2;
        var height = _backgroundHeight + _backgroundPadding.y * 2;
        _background.size = new Vector2(width, height);
        _background.transform.position = transform.position;
    }

    private void OnDestroy()
    {
        for (var i = 0; i < _slots.Count; i++)
            _slots[i] = null;
        _slots.Clear();
        _eventBus.Unsubscribe<GameEvent_SpawnSorterSlotsRequest>(OnSpawnRequest);
    }
}
