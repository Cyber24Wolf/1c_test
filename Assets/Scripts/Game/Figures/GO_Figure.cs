using Cysharp.Threading.Tasks;
using LitMotion;
using R3;
using System;
using UnityEngine;

public class GO_Figure : MonoBehaviour
{
    [SerializeField] private SpriteRenderer     _renderer;

    private CompositeDisposable _disposables = new CompositeDisposable();
    private IAnimator_GO_Figure _animator;

    public ViewModel Model { get; private set; } = new();

    private void Awake()
    {
        _animator = GetComponent<IAnimator_GO_Figure>();
    }

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
        if (dt <= 0f)
            return;

        if (Model == null)
            return;

        if (Model.ManualControl.Value == true)
            return;

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
        _animator.ShowAnimation(null);
    }

    private void OnShowCommand(ShowInput input)
    {
        if (_animator == null)
            return;

        _animator.ShowAnimation(input.OnShowComplete);
    }

    private void OnHideCommand(HideInput input)
    {
        if (_animator == null)
            return;

        if (input.Explode)
            _animator.ExplodeAnimation(input.OnHideComplete);
        else 
            _animator.HideAnimation(input.OnHideComplete);
    }

    public class ViewModel
    {
        public ReactiveProperty<DO_Figure> FigureData    { get; private set; } = new();
        public ReactiveProperty<bool>      ManualControl { get; private set; } = new();
        public ReactiveProperty<Vector2>   Velocity      { get; private set; } = new();
        public ReactiveCommand<HideInput>  HideCommand   { get; private set; } = new();
        public ReactiveCommand<ShowInput>  ShowCommand   { get; private set; } = new();
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
