using DG.Tweening;
using UnityEngine;

public class DynamicMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource _intro;
    [SerializeField] private AudioSource _loop;
    [SerializeField] private AudioSource _outro;

    [SerializeField, Range(0, 1)] private float _originalLoopVolume = 1f;
    [SerializeField, Range(0, 1)] private float _originalOutroVolume = 1f;

    private bool _isPlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPlaying();
    }

    public void StartPlaying()
    {
        if (_isPlaying)
            return;

        _loop.volume = _originalLoopVolume;

        _intro.Play();
        _loop.PlayDelayed(_intro.clip.length - .01f);

        _isPlaying = true;
    }

    public void EndPlaying()
    {
        if (!_isPlaying)
            return;

        _outro.volume = 0f;
        _outro.Play();

        _loop.DOFade(0f, 1f).OnComplete(() => _loop.Stop());
        _outro.DOFade(_originalOutroVolume, 1f);
        _isPlaying = false;
    }
}
