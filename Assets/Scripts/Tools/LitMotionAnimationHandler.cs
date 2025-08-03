using Cysharp.Threading.Tasks;
using LitMotion.Animation;
using System;

public static class LitMotionAnimationHandler
{
    public static async UniTask PlayAsync(LitMotionAnimation animation, Action onComplete = null)
    {
        if (animation == null)
        {
            onComplete?.Invoke();
            return;
        }

        animation.Play();
        while (animation.IsPlaying)
            await UniTask.Yield();
        onComplete?.Invoke();
    }
}
