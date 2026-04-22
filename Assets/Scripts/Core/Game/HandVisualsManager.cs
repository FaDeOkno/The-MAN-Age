using UnityEngine;

public class HandVisualsManager : MonoBehaviour
{
    public static HandVisualsManager Instance { get; private set; }

    [SerializeField] private GameEvent _setActiveEvent;
    [SerializeField] private GameEvent _setInactiveEvent;

    private HandAnimation _currentAnimation = HandAnimation.None;
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
        PlayAnimation(HandAnimation.None);
    }

    public void PlayAnimation(HandAnimation animation)
    {
        if (_currentAnimation == animation)
            return;

        if (_awaitingAnimationFinish)
            return;

        _setInactiveEvent.Raise(this, _currentAnimation);
        _setActiveEvent.Raise(this, animation);

        _currentAnimation = animation;
        _awaitingAnimationFinish = true;
    }

    public void OnAnimationFinished()
    {
        if (!_awaitingAnimationFinish)
            return;

        _awaitingAnimationFinish = false;

        if (_currentAnimation == HandAnimation.GettingId)
        {
            Debug.Log("Got ID");
            _hasId = true;
            PlayAnimation(HandAnimation.ShowingId);
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
