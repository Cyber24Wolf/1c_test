using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System;
using System.Threading;
using UnityEngine;

public interface IAnimator_GO_Figure
{
    void ShowAnimation(Action onComplete);
    void HideAnimation(Action onComplete);
    void ExplodeAnimation(Action onComplete);
}

public class Animator_GO_Figure : MonoBehaviour, IAnimator_GO_Figure
{
    [SerializeField] private Transform _target;
    [SerializeField] private SpriteRenderer _renderer;

    [Header("Show")]
    [SerializeField] private float _showDuration = 0.15f;

    [Header("Hide")]
    [SerializeField] private float _hideDuration = 0.15f;

    [Header("Explode")]
    [SerializeField] private float _explodeDuration = 0.5f;
    [SerializeField] private float _explodeScale    = 3f;


    private CancellationTokenSource _cts;

    public void ShowAnimation(Action onComplete)
    {
        GenereteCTS();
        ShowAnimationAsync(onComplete, _cts.Token).Forget();
    }

    public void HideAnimation(Action onComplete)
    {
        GenereteCTS();
        HideAnimationAsync(onComplete, _cts.Token).Forget();
    }

    public void ExplodeAnimation(Action onComplete)
    {
        GenereteCTS();
        ExplodeAnimationAsync(onComplete, _cts.Token).Forget();
    }

    private async UniTask ShowAnimationAsync(Action onComplete, CancellationToken token)
    {
        try
        {
            await LSequence.Create()
                .Append(LMotion.Create(0f, 1f, _showDuration)
                               .WithEase(Ease.InSine)
                               .BindToLocalScaleX(_target))
                .Join(LMotion.Create(0f, 1f, _showDuration)
                             .WithEase(Ease.InSine)
                             .BindToLocalScaleY(_target))
                .Join(LMotion.Create(0f, 1f, _showDuration)
                             .WithEase(Ease.InSine)
                             .BindToColorA(_renderer))
                .Run()
                .ToAwaitable(token);
        }
        finally
        {
            onComplete?.Invoke();
        }
    }

    private async UniTask HideAnimationAsync(Action onComplete, CancellationToken token)
    {
        try
        {
            await LSequence.Create()
                .Append(LMotion.Create(1f, 0f, _hideDuration)
                               .WithEase(Ease.InSine)
                               .BindToLocalScaleX(_target))
                .Join(LMotion.Create(1f, 0f, _hideDuration)
                             .WithEase(Ease.InSine)
                             .BindToLocalScaleY(_target))
                .Join(LMotion.Create(1f, 0f, _hideDuration)
                             .WithEase(Ease.InSine)
                             .BindToColorA(_renderer))
                .Run()
                .ToAwaitable(token);
        }
        finally
        {
            onComplete?.Invoke();
        }
    }

    private async UniTask ExplodeAnimationAsync(Action onComplete, CancellationToken token)
    {
        try
        {
            await LSequence.Create()
                .Append(LMotion.Create(1f, _explodeScale, _explodeDuration)
                               .WithEase(Ease.InSine)
                               .BindToLocalScaleX(_target))
                .Join(LMotion.Create(1f, _explodeScale, _explodeDuration)
                             .WithEase(Ease.InSine)
                             .BindToLocalScaleY(_target))
                .Join(LMotion.Create(1f, 0f, _hideDuration)
                             .WithEase(Ease.InSine)
                             .BindToColorA(_renderer))
                .Run()
                .ToAwaitable(token);
        }
        finally
        {
            onComplete?.Invoke();
        }
    }

    private void GenereteCTS()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
        _cts = new CancellationTokenSource();
    }
}
