using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GO_Sorter : MonoBehaviour
{
    [SerializeField] float _distanceBetweenSlots = 1.5f;
    [SerializeField] Transform _background;

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
        Debug.Log($"Sorter slots created: {_slots.Count}");
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
            var pos = transform.position + transform.right * (offset * _distanceBetweenSlots + adjust);
            _slots[i].transform.position = pos;
        }

        ResizeBackground(_background, _slots.Count, _distanceBetweenSlots);
    }

    private void ResizeBackground(Transform background, int count, float spacing, float padding = 1f)
    {
        var width = (count - 1) * spacing + padding * 2;
        var height = padding;
        background.localScale = new Vector3(width, height, background.localScale.z);
        background.position = transform.position;
    }

    private void OnDestroy()
    {
        _slots.Clear();
        _eventBus.Unsubscribe<GameEvent_SpawnSorterSlotsRequest>(OnSpawnRequest);
    }
}
