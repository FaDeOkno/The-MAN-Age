using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimationOnEvent : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string _animationId;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Play(Component sender, object param)
    {
        _animator.Play(_animationId);
    }
}
