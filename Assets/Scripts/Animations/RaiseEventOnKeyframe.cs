using UnityEngine;

public class RaiseEventOnKeyframe : MonoBehaviour
{
    [SerializeField] private GameEvent _eventToRaise;

    public void RaiseEvent()
    {
        _eventToRaise.Raise(this, null);
    }
}
