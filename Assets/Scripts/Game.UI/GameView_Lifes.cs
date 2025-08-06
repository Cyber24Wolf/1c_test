using LitMotion;
using TMPro;
using UnityEngine;
using Zenject;

public class GameView_Lifes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoresLabel;

    private EventBus     _eventBus;
    private ILifeService _lifeService;

    [Inject]
    private void Setup(EventBus eventBus, ILifeService lifeService)
    {
        _eventBus    = eventBus;
        _lifeService = lifeService;

        _eventBus.Subscribe<GameEvent_OnSetLifes>(OnLifesSet);
        _eventBus.Subscribe<GameEvent_OnDealDamage>(OnDealDamage);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GameEvent_OnSetLifes>(OnLifesSet);
        _eventBus.Unsubscribe<GameEvent_OnDealDamage>(OnDealDamage);
    }

    private void OnLifesSet(GameEvent_OnSetLifes e)
    {
        _scoresLabel.text = e.NewValue.ToString();
    }

    private void OnDealDamage(GameEvent_OnDealDamage e)
    {
        _scoresLabel.text = e.NewValue.ToString();
    }
}
