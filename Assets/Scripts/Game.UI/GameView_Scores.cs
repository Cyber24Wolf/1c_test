using LitMotion;
using TMPro;
using UnityEngine;
using Zenject;

public class GameView_Scores : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoresLabel;

    private EventBus     _eventBus;
    private ILifeService _lifeService;

    [Inject]
    private void Setup(EventBus eventBus, ILifeService lifeService)
    {
        _eventBus    = eventBus;
        _lifeService = lifeService;

        _eventBus.Subscribe<GameEvent_OnScoresSet>(OnScoresSet);
        _eventBus.Subscribe<GameEvent_OnScoresAdd>(OnScoresAdd);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GameEvent_OnScoresSet>(OnScoresSet);
        _eventBus.Unsubscribe<GameEvent_OnScoresAdd>(OnScoresAdd);
    }

    private void OnScoresSet(GameEvent_OnScoresSet e)
    {
        _scoresLabel.text = e.NewValue.ToString();
    }

    private void OnScoresAdd(GameEvent_OnScoresAdd e)
    {
        _scoresLabel.text = e.NewValue.ToString();
    }
}
