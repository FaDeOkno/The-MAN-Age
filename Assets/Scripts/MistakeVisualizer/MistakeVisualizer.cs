using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MistakeVisualizer : MonoBehaviour
{
    [SerializeField] private int _mistakesCount = 1;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnEvent(Component sender, object data)
    {
        if (data is not int count)
            return;

        _animator.SetBool("IsActive", count >= _mistakesCount);
    }
}
