using UnityEngine;

public class HandVisualsManager : MonoBehaviour
{
    public static HandVisualsManager Instance { get; private set; }

    [SerializeField] private Animator _gettingIdHand;
    [SerializeField] private Animator _startShowingIdHand;
    [SerializeField] private Animator _showingIdHand;
    [SerializeField] private Animator _showingNewsHand;

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

        switch (_currentAnimation)
        {
            case HandAnimation.ShowingId:
                _showingIdHand.SetBool("IsActive", false);
                break;
            case HandAnimation.ShowingNews:
                _showingNewsHand.SetBool("IsActive", false);
                break;
            default:
                break;
        }

        switch (animation)
        {
            case HandAnimation.GettingId:
                _gettingIdHand.Play("GetId");
                break;
            case HandAnimation.ShowingId:
                _startShowingIdHand.SetBool("IsActive", true);
                break;
            case HandAnimation.ShowingNews:
                _showingNewsHand.SetBool("IsActive", true);
                break;
            default:
                break;
        }

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
        else if (_currentAnimation == HandAnimation.ShowingId)
        {
            _startShowingIdHand.SetBool("IsActive", false);
            _startShowingIdHand.Play("NoHand");

            _showingIdHand.SetBool("IsActive", true);
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
