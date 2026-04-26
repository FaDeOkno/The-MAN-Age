using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableId : MonoBehaviour, IPointerClickHandler
{
    public float ScaleMultiplier = .6f;
    [SerializeField] private GameEvent _setTargetEvent;
    [SerializeField] private Transform _originalFollowPoint;

    public bool IsActive = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        IsActive = !IsActive;

        if (IsActive)
        {
            _setTargetEvent.Raise(this, gameObject.transform);
        }
        else
        {
            _setTargetEvent.Raise(this, _originalFollowPoint);
        }
    }

    public void Unfocus()
    {
        if (!IsActive)
            return;

        _setTargetEvent.Raise(this, _originalFollowPoint);
    }
}
