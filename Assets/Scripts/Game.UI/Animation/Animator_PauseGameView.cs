using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System;
using System.Threading;
using UnityEngine;

public interface IAnimator_PauseGameView
{
    void ShowAnimation(Action onComplete);
    void HideAnimation(Action onComplete);
}

public class Animator_PauseGameView : MonoBehaviour, IAnimator_PauseGameView
{
    [Header("Roots")]
    [SerializeField] private RectTransform _leftBg;
    [SerializeField] private RectTransform _rightBg;
    [SerializeField] private RectTransform _header;
    [SerializeField] private RectTransform _startButtonRoot;

    [Header("Show.Timings")]
    [SerializeField] private float _showDuration_bg = 0.2f;
    [SerializeField] private float _showDuration_header = 0.15f;
    [SerializeField] private float _showDuration_startButton = 0.15f;

    [Header("Hide.Timings")]
    [SerializeField] private float _hideDuration_bg = 0.5f;
    [SerializeField] private float _hideDuration_header = 0.5f;
    [SerializeField] private float _hideDuration_startButton = 0.5f;

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

    private async UniTask ShowAnimationAsync(Action onComplete, CancellationToken token)
    {
        Debug.Log("Animator_PauseGameView: ShowAnimationAsync");
        try
        {
            await LSequence
                .Create()
                .Append(LMotion.Create(0f, 1f, _showDuration_bg).BindToLocalScaleX(_leftBg))
                .Join(LMotion.Create(0f, 1f, _showDuration_bg).BindToLocalScaleX(_rightBg))
                .Append(LMotion.Create(0f, 1f, _showDuration_header).BindToLocalScaleX(_header))
                .Join(LMotion.Create(0f, 1f, _showDuration_header).BindToLocalScaleY(_header))
                .Join(LMotion.Create(0f, 1f, _showDuration_startButton).BindToLocalScaleX(_startButtonRoot))
                .Join(LMotion.Create(0f, 1f, _showDuration_startButton).BindToLocalScaleY(_startButtonRoot))
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
            await LSequence
                .Create()
                .Append(LMotion.Create(1f, 0f, _hideDuration_bg).BindToLocalScaleX(_leftBg))
                .Join(LMotion.Create(1f, 0f, _hideDuration_bg).WithEase(Ease.InSine).BindToLocalScaleX(_rightBg))
                .Append(LMotion.Create(1f, 0f, _hideDuration_header).BindToLocalScaleX(_header))
                .Join(LMotion.Create(1f, 0f, _hideDuration_header).BindToLocalScaleY(_header))
                .Join(LMotion.Create(1f, 0f, _hideDuration_startButton).BindToLocalScaleX(_startButtonRoot))
                .Join(LMotion.Create(1f, 0f, _hideDuration_startButton).BindToLocalScaleY(_startButtonRoot))
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
