using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HandVisuals : MonoBehaviour
{
    [SerializeField] private HandAnimation _visualsType;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnToggleOn(Component source, object data)
    {
        if (data is not HandAnimation animation)
            return;

        if (animation != _visualsType)
            return;

        if (animation == HandAnimation.GettingId)
            _animator.Play("GetId");
        else
            _animator.SetBool("IsActive", true);
    }

    public void OnToggleOff(Component source, object data)
    {
        if (data is not HandAnimation animation)
            return;

        if (animation != _visualsType)
            return;

        if (animation == HandAnimation.GettingId)
            return;

        _animator.SetBool("IsActive", false);
    }
}
