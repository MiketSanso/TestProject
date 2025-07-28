using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class CubeView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rectTransform;
    
    [Inject] private readonly SignalBus _signalBus;
    private Vector2 _dragOffset;
    private bool _isDragging;
    private CanvasGroup _canvasGroup;
    
    public RectTransform RectTransform => _rectTransform;
    public Color Color => _image.color;
    public int Index { get; private set; }

    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    public void Initialize(Color color, int index)
    {
        _image.color = color;
        Index = index;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        
        _dragOffset = _rectTransform.anchoredPosition - localPoint;
        _canvasGroup.blocksRaycasts = false;
        _signalBus.Fire(new ScrollLockSignal { Locked = true });
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );
        
        _rectTransform.anchoredPosition = localPoint + _dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        _canvasGroup.blocksRaycasts = true;
        _signalBus.Fire(new ScrollLockSignal { Locked = false });
    }

    public void MoveToPosition(Vector2 position, float duration = 0.3f)
    {
        _rectTransform.DOAnchorPos(position, duration).SetEase(Ease.OutBack);
    }

    public void Destroy()
    {
        _rectTransform.DOScale(Vector3.zero, 0.3f)
            .OnComplete(() => Destroy(gameObject));
    }
}