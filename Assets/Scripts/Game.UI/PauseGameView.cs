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

    private void SubscribeEvents()
    {
        if (_eventBus == null)
            return;

        _eventBus.Subscribe<GameEvent_Loose>(OnLoose);
        _eventBus.Subscribe<GameEvent_Win>(OnWin);
    }

    private void UnsubscribeEvents()
    {
        _eventBus.Unsubscribe<GameEvent_Loose>(OnLoose);
        _eventBus.Unsubscribe<GameEvent_Win>(OnWin);
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

    private void OnLoose(GameEvent_Loose e) => OnGameFinished();
    private void OnWin(GameEvent_Win e) => OnGameFinished();

    private void OnGameFinished()
    {
        _startButton.interactable = true;
        if (_animator != null)
            _animator.ShowAnimation(null);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}
