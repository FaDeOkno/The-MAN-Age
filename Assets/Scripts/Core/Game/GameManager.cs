using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int Year = 984;
    public static GameManager Instance { get; private set; }

    public int Seed = 0;

    [SerializeField, Header("General settings")] private DayData[] _days;
    [SerializeField] private int _visitorsCap = 15;
    [SerializeField] private int _mistakeCap = 3;
    [SerializeField] private Vector2 _spawnOffset = Vector2.down * 8;
    [SerializeField] private Vector2 _characterOffset = Vector2.down;

    [SerializeField, Header("Events")] private GameEvent VisitorCheckedEvent;
    [SerializeField] private GameEvent MistakeEvent;
    [SerializeField] private GameEvent UpdateNewspaperEvent;
    [SerializeField] private GameEvent NextDayStartingEvent;
    [SerializeField] private GameEvent FailureEvent;

    [SerializeField, Header("Prefabs")] private GameObject VisitorPrefab;
    [SerializeField] private GameObject _introVisitorPrefab;
    [SerializeField] private Transform _gameplayTransform;

    [SerializeField, Header("SFX")] private AudioSource _successJingle;
    [SerializeField] private AudioSource _correctJingle;
    [SerializeField] private AudioSource _failureJingle;
    [SerializeField] private AudioSource _mistakeJingle;

    private Visitor _currentVisitor;
    private int _curVisitorIndex = 0;
    private bool _isFading = true;

    public int CurDayIndex = 0;
    private int _mistakes = 0;
    private DayData _curDay => CurDayIndex >= _days.Count() ? _days.Last() : _days[CurDayIndex];

    private System.Random _random;
    private int _seed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (Instance != null && Instance != this)
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
        Invoke("NextVisitorOrEndDay", 2f);
    }

    public void NextVisitorOrEndDay()
    {
        _curVisitorIndex++;
        _seed = Seed + _curVisitorIndex + (CurDayIndex * _curDay.VisitorCount);
        _random = new System.Random(_seed);

        if (_curVisitorIndex <= 1)
        {
            UpdateNewspaperEvent.Raise(this, _curDay);

            if (_curDay.InitialDialogue != null)
                SpawnIntroVisitor();
            else
                SpawnVisitor();
        }
        else if (_mistakeCap <= _mistakes)
        {
            EndDay(false);
        }
        else if (_curVisitorIndex <= _curDay.VisitorCount)
        {
            SpawnVisitor();
        }
        else
        {
            EndDay();
        }
    }

    private void SpawnIntroVisitor()
    {
        var visitor = Instantiate(_introVisitorPrefab, _gameplayTransform).GetComponent<Visitor>();

        _currentVisitor = visitor;

        visitor.PlaySpawnAnimation(_spawnOffset, _characterOffset, () =>
        {
            _isFading = false;
            var dialogueCaller = visitor.GetComponent<DialogueCaller>();
            dialogueCaller.Dialogue = _curDay.InitialDialogue;
            dialogueCaller.OnStartEvent();
        });
    }

    private void SpawnVisitor()
    {
        var visitor = Instantiate(VisitorPrefab, _gameplayTransform).GetComponent<Visitor>();
        _currentVisitor = visitor;

        visitor.Generate(_seed, _random.Pick(_curDay.Species));
        visitor.PlaySpawnAnimation(_spawnOffset, _characterOffset, () => _isFading = false);
    }

    public void VisitorApprove()
    {
        if (_isFading)
            return;

        _isFading = true;
        VisitorCheckedEvent.Raise(this, null);

        var success = _currentVisitor.IsValid || _currentVisitor.AlwaysValid;
        if (success)
            _correctJingle.Play();
        else
            _mistakeJingle.Play();

        _currentVisitor.FadeColor();
        _currentVisitor.transform.DOMoveX(_currentVisitor.transform.position.x - 16, 2f).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                if (!success)
                    DoMistake();

                Destroy(_currentVisitor.gameObject);
                NextVisitorOrEndDay();
            });
    }

    public void VisitorDisapprove()
    {
        if (_isFading)
            return;

        _isFading = true;
        VisitorCheckedEvent.Raise(this, null);

        var success = !_currentVisitor.IsValid || _currentVisitor.AlwaysValid;
        if (success)
            _correctJingle.Play();
        else
            _mistakeJingle.Play();

        _currentVisitor.FadeColor();
        _currentVisitor.transform.DOMoveX(_currentVisitor.transform.position.x + 16, 2f).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                if (_currentVisitor.IsValid && !_currentVisitor.AlwaysValid)
                    DoMistake();

                Destroy(_currentVisitor.gameObject);
                NextVisitorOrEndDay();
            });
    }

    public void StartDay()
    {
        if (_currentVisitor != null)
            return;

        _curVisitorIndex = 0;
        _mistakes = 0;
        MistakeEvent.Raise(this, 0);

        Invoke("NextVisitorOrEndDay", 2f);
    }

    public void EndDay(bool success = true)
    {
        if (success)
        {
            _successJingle?.Play();
            CurDayIndex++;

            NextDayStartingEvent.Raise(this, CurDayIndex);
        }
        else
        {
            _failureJingle?.Play();
            FailureEvent.Raise(this, null);
        }
    }

    private void DoMistake()
    {
        _mistakes++;
        MistakeEvent.Raise(this, _mistakes);
    }
}

public enum GameplayState
{
    Visitor,
    Table
}
