using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseGameView : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private EventBus _eventBus;
    private IAnimator_PauseGameView _animator;

    private void Awake()
    {
        _animator = GetComponent<IAnimator_PauseGameView>();
    }

    [Inject]
    private void Setup(EventBus eventBus)
    {
        _eventBus = eventBus;


        SubscribeEvents();
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        if (_eventBus == null)
            return;

        ProceedHideAnimation(OnHideFinished);
    }

    private void ProceedHideAnimation(Action onComplete)
    {
        _startButton.interactable = false;
        if (_animator != null)
            _animator.HideAnimation(OnHideFinished);
    }

    private void OnHideFinished()
    {
        _eventBus.Publish(new GameEvent_StartGameRequest());
    }

    private void OnWin(GameEvent_GameFinished e)
        => OnGameFinished(e);

    private void OnGameFinished(GameEvent_GameFinished e)
    {
        if (_animator != null)
            _animator.ShowAnimation(OnAnimationShow);
    }

    private void OnAnimationShow()
    {
        _startButton.interactable = true;
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        if (_eventBus == null)
            return;

        _eventBus.Subscribe<GameEvent_GameFinished>(OnWin);
    }

    private void UnsubscribeEvents()
    {
        _eventBus.Unsubscribe<GameEvent_GameFinished>(OnWin);
    }
}
