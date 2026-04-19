using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Seed = 0;
    [SerializeField] private int _visitorsCap = 15;
    [SerializeField] private int _mistakeCap = 3;
    [SerializeField] private GameObject VisitorPrefab;
    [SerializeField] private GameEvent VisitorCheckedEvent;
    [SerializeField] private Transform _gameplayTransform;
    [SerializeField] private Species[] _species;

    [SerializeField] private Vector2 _spawnOffset = Vector2.down * 8;
    [SerializeField] private Vector2 _characterOffset = Vector2.down;

    private Visitor _currentVisitor;
    private int _curVisitorIndex = 0;
    private bool _isFading = true;
    private int _curDay = 0;
    private int _mistakes = 0;
    private System.Random _random;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        NextVisitorOrEndDay();
    }

    public void NextVisitorOrEndDay()
    {
        _curVisitorIndex++;
        _random = new System.Random(Seed + _curVisitorIndex + (_curDay * _visitorsCap));

        if (_curVisitorIndex <= _visitorsCap)
        {
            SpawnVisitor();
        }
        else
        {
            EndDay();
        }
    }

    private void SpawnVisitor()
    {
        var visitor = Instantiate(VisitorPrefab, _gameplayTransform).GetComponent<Visitor>();
        _currentVisitor = visitor;

        visitor.Generate(_random, _random.Pick(_species));
        visitor.PlaySpawnAnimation(_spawnOffset, _characterOffset, () => _isFading = false);
    }

    public void VisitorApprove()
    {
        if (_isFading)
            return;

        _isFading = true;
        VisitorCheckedEvent.Raise(this, null);

        _currentVisitor.FadeColor();
        _currentVisitor.transform.DOMoveX(_currentVisitor.transform.position.x - 16, 2f).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
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

        _currentVisitor.FadeColor();
        _currentVisitor.transform.DOMoveX(_currentVisitor.transform.position.x + 16, 2f).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                Destroy(_currentVisitor.gameObject);
                NextVisitorOrEndDay();
            });
    }

    public void StartDay()
    {
        Debug.Log("Day started!");
        _curVisitorIndex = 0;
        NextVisitorOrEndDay();
    }

    public void EndDay()
    {
        Debug.Log("Day ended!");
        // Implement end of day logic here (e.g., show summary, reset for next day, etc.)
    }
}

public enum GameplayState
{
    Visitor,
    Table
}
