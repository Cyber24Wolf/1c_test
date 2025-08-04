using R3;
using UnityEngine;

public class GO_SorterSlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    private CompositeDisposable _disposables = new CompositeDisposable();

    public ViewModel Model { get; private set; } = new();

    private void OnEnable()
    {
        _disposables.Clear();
        Model.FigureData
             .Subscribe(OnFigureDataChanged)
             .AddTo(_disposables);
    }

    private void OnFigureDataChanged(DO_Figure figureData)
    {
        if (figureData == null)
        {
            _renderer.sprite = null;
            return;
        }
        _renderer.sprite = figureData.sprite;
    }

    private void OnDisable() => _disposables.Clear();
    private void OnDestroy() => _disposables.Dispose();

    public class ViewModel
    {
        public ReactiveProperty<DO_Figure> FigureData { get; private set; } = new();
    }
}
