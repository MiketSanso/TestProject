using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScrollController : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    
    [Inject] private readonly GameModel _gameModel;
    [Inject] private readonly SignalBus _signalBus;
    
    private CompositeDisposable _disposables = new();

    private void Start()
    {
        _signalBus.GetStream<ScrollLockSignal>()
            .Subscribe(signal => _scrollRect.enabled = !signal.Locked)
            .AddTo(_disposables);
        
        _scrollRect.onValueChanged.AsObservable()
            .Subscribe(pos => _gameModel.ScrollPosition.Value = pos.x)
            .AddTo(_disposables);
        
        _gameModel.ScrollPosition
            .Subscribe(pos => _scrollRect.horizontalNormalizedPosition = pos)
            .AddTo(_disposables);
    }

    private void OnDestroy() => _disposables.Dispose();
}