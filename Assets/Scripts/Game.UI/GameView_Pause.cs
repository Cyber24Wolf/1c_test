using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameView_Pause : MonoBehaviour
{
    [SerializeField] private Button          _startButton;
    [SerializeField] private TextMeshProUGUI _headerLabel;
    [SerializeField] private TextMeshProUGUI _scoresInfo;

    [SerializeField] private Color  _winColor     = Color.green;
    [SerializeField] private Color  _looseColor   = Color.red;
    [SerializeField] private string _winText      = "WIN";
    [SerializeField] private string _looseText    = "LOOSE";
    [SerializeField] private string _scoresFormat = "Scores: {0}";

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
        _eventBus.Subscribe<GameEvent_GameFinished>(OnGameFinished);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GameEvent_GameFinished>(OnGameFinished);
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

        _eventBus.Publish(new GameEvent_StartGameRequest());
        ProceedHideAnimation(null);
    }

    private void ProceedHideAnimation(Action onComplete)
    {
        _startButton.interactable = false;
        if (_animator != null)
            _animator.HideAnimation(null);
    }

    private void OnGameFinished(GameEvent_GameFinished e)
    {
        _headerLabel.text  = e.IsWin ? _winText  : _looseText;
        _headerLabel.color = e.IsWin ? _winColor : _looseColor;

        _scoresInfo.text = string.Format(_scoresFormat, e.Scores);

        if (_animator != null)
            _animator.ShowAnimation(OnAnimationShow);
    }

    private void OnAnimationShow()
    {
        _startButton.interactable = true;
    }
}
