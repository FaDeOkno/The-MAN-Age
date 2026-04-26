using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicMusicController : MonoBehaviour
{
    [SerializeField] private DynamicMusicLayer[] _layers;
    [SerializeField] private int _currentLayer = 0;

    [SerializeField, Range(0, 1)] private float _originalLoopVolume = 1f;
    [SerializeField, Range(0, 1)] private float _originalOutroVolume = 1f;

    private bool _isPlaying;
    public DynamicMusicLayer CurrentLayer => _layers[_currentLayer];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPlaying();
    }

    public void StartPlaying()
    {
        if (_isPlaying)
            return;

        if (CurrentLayer.Outro.isPlaying)
        {
            CurrentLayer.Outro.DOFade(0, 1.5f);
        }

        //SilenceEverything();
        StartSyncLoops();

        _isPlaying = true;
    }

    public void EndPlaying()
    {
        if (!_isPlaying)
            return;

        _isPlaying = false;

        EndEverything();
    }

    public void SwitchLayers(int newLayer)
    {
        if (_currentLayer == newLayer)
            return;

        CurrentLayer.Intro.DOFade(0, 2f);
        CurrentLayer.Loop.DOFade(0, 2f);
        CurrentLayer.Outro.DOFade(0, 2f);

        _currentLayer = newLayer;

        CurrentLayer.Intro.DOFade(_originalLoopVolume, 2f);
        CurrentLayer.Loop.DOFade(_originalLoopVolume, 2f);
        CurrentLayer.Outro.DOFade(_originalOutroVolume, 2f);
    }

    public void OnSwitchEvent(Component sender, object data)
    {
        if (data is not int layer)
            return;

        if (_layers.Length <= layer || layer < 0)
            return;

        SwitchLayers(layer);
    }

    private void StartSyncLoops()
    {
        foreach (var layer in _layers)
        {
            layer.Intro.volume = layer == CurrentLayer ? _originalLoopVolume : 0f;
            layer.Loop.volume = layer == CurrentLayer ? _originalLoopVolume : 0f;
            layer.Outro.volume = 0f;

            layer.Intro.Play();
            layer.Loop.PlayDelayed(layer.Intro.clip.length - .01f);
        }
    }

    private void StopEverything()
    {
        foreach (var layer in _layers)
        {
            layer.Intro.Stop();
            layer.Loop.Stop();
            layer.Outro.Stop();
        }
    }

    private void EndEverything()
    {
        foreach (var layer in _layers)
        {
            layer.Outro.volume = layer == CurrentLayer ? _originalOutroVolume : 0;
            layer.Outro.Play();

            layer.Loop.DOFade(0f, 1f).OnComplete(() => layer.Loop.Stop());
            layer.Outro.DOFade(_originalOutroVolume, 1f);
        }
    }

    private void SilenceEverything()
    {
        foreach (var layer in _layers)
        {
            layer.Intro.volume = 0;
            layer.Loop.volume = 0;
            layer.Outro.volume = 0;
        }
    }
}

[System.Serializable]
public class DynamicMusicLayer
{
    [SerializeField] public AudioSource Intro;
    [SerializeField] public AudioSource Loop;
    [SerializeField] public AudioSource Outro;
}
