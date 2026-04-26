using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeObject : MonoBehaviour
{
    private CanvasGroup _group;
    [SerializeField] private bool _introFade = true;
    [SerializeField] private float _introFadeDelay = 0f;
    [SerializeField] private GameEvent _fadeInEvent;
    [SerializeField] private GameEvent _fadeOutEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _group = GetComponent<CanvasGroup>();

        if (!_introFade)
            return;

        _group.alpha = 1f;
        Invoke("FadeOutNoEvent", _introFadeDelay);
    }

    public void FadeIn()
    {
        if (_group.alpha >= 1f)
            return;

        _group.DOFade(1f, 2f).SetEase(Ease.InSine).OnComplete(() => { _fadeInEvent?.Raise(this, null); _group.interactable = true; _group.blocksRaycasts = true; });
    }

    public void FadeOut()
    {
        if (_group.alpha <= 0f)
            return;

        _group.interactable = false;
        _group.blocksRaycasts = false;
        _group.DOFade(0f, 2f).SetEase(Ease.InSine).OnComplete(() => { _fadeOutEvent?.Raise(this, null); });
    }

    public void FadeOutNoEvent()
    {
        if (_group.alpha <= 0f)
            return;

        _group.interactable = false;
        _group.DOFade(0f, 2f).SetEase(Ease.InSine);
    }
}
