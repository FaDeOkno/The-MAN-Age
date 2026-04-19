using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private Animator _animator;

    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private GameEvent _onInteractEvent;

    private float _cooldownTimer = 0f;
    public bool IsHovered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovered = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_cooldownTimer > 0)
            return;

        _animator.Play("ButtonPress");
        _onInteractEvent.Raise(this, null);
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        _animator.SetBool("IsHovered", IsHovered);
    }
}
