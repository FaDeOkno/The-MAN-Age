using UnityEngine;

public class HandVisualsManager : MonoBehaviour
{
    public static HandVisualsManager Instance { get; private set; }

    [SerializeField] private GameEvent _setActiveEvent;
    [SerializeField] private GameEvent _setInactiveEvent;

    public HandAnimation CurrentAnimation { get; private set; } = HandAnimation.None;
    private bool _awaitingAnimationFinish = false;
    private bool _hasId = false;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnGetId(Component sender, object data)
    {
        PlayAnimation(HandAnimation.GettingId);
    }

    public void OnOpenNews(Component sender, object data)
    {
        PlayAnimation(HandAnimation.ShowingNews);
    }

    public void OnCloseNews(Component sender, object data)
    {
        PlayAnimation(_hasId ? HandAnimation.ShowingId : HandAnimation.None);
    }

    public void OnVisitorChecked(Component sender, object data)
    {
        _hasId = false;

        if (CurrentAnimation == HandAnimation.ShowingNews)
            return;

        PlayAnimation(HandAnimation.None);
    }

    public void PlayAnimation(HandAnimation animation)
    {
        if (CurrentAnimation == animation)
            return;

        if (_awaitingAnimationFinish)
            return;

        _setInactiveEvent.Raise(this, CurrentAnimation);

        if (CurrentAnimation is HandAnimation.None or HandAnimation.GettingId)
            _setActiveEvent.Raise(this, animation);

        CurrentAnimation = animation;
        _awaitingAnimationFinish = true;
    }

    public void OnAnimationFinished()
    {
        if (!_awaitingAnimationFinish)
            return;

        _awaitingAnimationFinish = false;

        if (CurrentAnimation == HandAnimation.GettingId)
        {
            #if DEBUG
            Debug.Log("Got ID");
            #endif
            _hasId = true;
            PlayAnimation(HandAnimation.ShowingId);
        }
        else
        {
            _setActiveEvent.Raise(this, CurrentAnimation);
        }
    }
}

public enum HandAnimation
{
    None,
    GettingId,
    ShowingId,
    ShowingNews
}
