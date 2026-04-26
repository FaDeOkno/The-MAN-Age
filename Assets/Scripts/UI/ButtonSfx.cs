using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSfx : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioSource _hoverSfx;
    [SerializeField] private AudioSource _clickSfx;

    public void OnPointerEnter(PointerEventData data)
    {
        _hoverSfx.Play();
    }

    public void OnClick()
    {
        _clickSfx.Play();
    }
}
