using UnityEngine;
using Zenject;

public class GO_Draggable : MonoBehaviour
{
    [Inject] private IDragService _dragService;

    private void OnEnable()
    {
        if (_dragService == null)
            return;
        _dragService.Register(this);
    }

    private void OnDisable()
    {
        if (_dragService == null)
            return;
        _dragService.Unregister(this);
    }
}