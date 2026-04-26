using DG.Tweening;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] private int _musicLayer = 0;
    [SerializeField] private Transform _root;
    [SerializeField] private RectTransform _uiRoot;
    [SerializeField] private Vector2 _rootOffset;
    [SerializeField] private Vector2 _uiOffset;
    [SerializeField] private GameEvent _musicEvent;

    public void OnPressed()
    {
        _root.DOLocalMove(_rootOffset, 1.5f).SetEase(Ease.OutQuad);
        _uiRoot.DOLocalMove(_uiOffset, 1.5f).SetEase(Ease.OutQuad);
        _musicEvent.Raise(this, _musicLayer);
    }
}
