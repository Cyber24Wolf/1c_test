using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class FiguresSpreader : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPositions;

    private EventBus        _eventBus;
    private IFiguresFactory _figuresFactory;
    private IGameplayConfig _gameplayConfig;

    private CancellationTokenSource _spreadCTS;
    private int                     _figuresLeft;

    [Inject]
    public void Setup(
        EventBus eventBus,
        IFiguresFactory factory,
        IGameplayConfig gameplayConfig)
    {
        _eventBus       = eventBus;
        _figuresFactory = factory;
        _gameplayConfig = gameplayConfig;

        _eventBus.Subscribe<GameEvent_StartSpreadFigures>(OnStartSpreadFigures);
        _eventBus.Subscribe<GameEvent_StopSpreadFigures>(OnStopSpreadFigures);
    }

    private void OnStartSpreadFigures(GameEvent_StartSpreadFigures evt)
    {
        _spreadCTS?.Cancel();
        _spreadCTS?.Dispose();

        _spreadCTS = new CancellationTokenSource();

        _figuresLeft = evt.FiguresCount;
        ProceedSpreadingAsync(_spreadCTS.Token).Forget();
    }
    private void OnStopSpreadFigures(GameEvent_StopSpreadFigures evt) 
    {
        _figuresLeft = 0;
    }

    private async UniTask ProceedSpreadingAsync(CancellationToken token)
    {
        if (_gameplayConfig.FigureTypes.Count == 0)
            return;

        if (_spawnPositions.Count == 0)
            return;

        if (_figuresLeft == 0)
            return;

        if (enabled == false)
            return;

        while (_spreadCTS.Token.IsCancellationRequested == false && _figuresLeft > 0)
        {
            var dataIndex = Random.Range(0, _gameplayConfig.FigureTypes.Count);
            var posIndex  = Random.Range(0, _spawnPositions.Count);
            var velocityX = Random.Range(_gameplayConfig.FiguresVelocityMin.x, _gameplayConfig.FiguresVelocityMax.x);
            var velocityY = Random.Range(_gameplayConfig.FiguresVelocityMin.y, _gameplayConfig.FiguresVelocityMax.y);
            _figuresFactory.Spawn(
                _gameplayConfig.FigureTypes[dataIndex],
                _spawnPositions[posIndex].position + _gameplayConfig.FigureTypes[dataIndex].spawnOffset,
                new Vector2(velocityX, velocityY)
            );

            _figuresLeft -= 1;
            var waitTime = Random.Range(_gameplayConfig.FiguresSpawnTimeMin, _gameplayConfig.FiguresSpawnTimeMax);
            if (waitTime > 0)
                await UniTask.WaitForSeconds(waitTime, cancellationToken: token);
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GameEvent_StartSpreadFigures>(OnStartSpreadFigures);
        _eventBus.Unsubscribe<GameEvent_StopSpreadFigures>(OnStopSpreadFigures);
        _spreadCTS?.Cancel();
        _spreadCTS?.Dispose();
    }
}
