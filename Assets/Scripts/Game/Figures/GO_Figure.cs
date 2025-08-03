using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Animation;
using R3;
using System;
using UnityEngine;

public class GO_Figure : MonoBehaviour
{
    [SerializeField] private SpriteRenderer     _renderer;
    [SerializeField] private LitMotionAnimation _showAnimation;
    [SerializeField] private LitMotionAnimation _hideAnimation;
    [SerializeField] private LitMotionAnimation _explodeAnimation;

    private CompositeDisposable _disposables = new CompositeDisposable();

    public ViewModel Model { get; private set; } = new();

    private void OnEnable()
    {
        _disposables.Clear();

        Model.FigureData
             .Subscribe(OnFigureDataChanged)
             .AddTo(_disposables);
        Model.HideCommand
             .Subscribe(OnHideCommand)
             .AddTo(_disposables);
        Model.ShowCommand
             .Subscribe(OnShowCommand)
             .AddTo(_disposables);
    }

    private void OnDisable() => _disposables.Clear();
    private void OnDestroy() => _disposables.Dispose();

    private void Update()
    {
        var dt = Time.deltaTime;
        ProceedVelocity(dt);
    }

    private void ProceedVelocity(float dt)
    {
        var velocity = Model.Velocity.Value;
        if (velocity == Vector2.zero)
            return;

        transform.position += (Vector3)velocity * dt;
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

    private void OnShowCommand(ShowInput input)
    {
        LitMotionAnimationHandler
            .PlayAsync(_hideAnimation, input.OnShowComplete)
            .Forget();
    }

    private void OnHideCommand(HideInput input)
    {
        if (input.Explode)
            LitMotionAnimationHandler
                .PlayAsync(_explodeAnimation, input.OnHideComplete)
                .Forget();
        else 
            LitMotionAnimationHandler
                .PlayAsync(_hideAnimation, input.OnHideComplete)
                .Forget();
    }

    public class ViewModel
    {
        public ReactiveProperty<DO_Figure> FigureData  { get; private set; } = new();
        public ReactiveProperty<Vector2>   Velocity    { get; private set; } = new();
        public ReactiveCommand<HideInput>  HideCommand { get; private set; } = new();
        public ReactiveCommand<ShowInput>  ShowCommand { get; private set; } = new();
    }

    public readonly struct HideInput
    {
        public bool   Explode { get; }
        public Action OnHideComplete  { get; }

        public HideInput(bool explode, Action onHide)
        {
            Explode        = explode;
            OnHideComplete = onHide;
        }
    }

    public readonly struct ShowInput
    {
        public Action OnShowComplete { get; }

        public ShowInput(Action onShow)
        {
            OnShowComplete = onShow;
        }
    }
}
