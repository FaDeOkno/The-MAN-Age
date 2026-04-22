using UnityEngine;

public class VisitorPhoto : MonoBehaviour
{
    [SerializeField] private GameObject LayerPrefab;

    private int _currentSeed;
    private GameObject _currentPhoto;

    private int _queuedSeed;
    private Species _queuedSpecies;

    public void Generate(Component sender, object data)
    {
        if (data is not Visitor visitor)
        {
            Debug.LogError("Expected Visitor data for VisitorPhoto generation");
            return;
        }

        Debug.Log($"Generating photo for visitor with seed {visitor.Seed} and species {visitor.Species}");

        _queuedSeed = visitor.Seed;
        _queuedSpecies = visitor.Species;

        if (HandVisualsManager.Instance.CurrentAnimation != HandAnimation.ShowingId)
            QueueMove();
    }

    public void QueueMove()
    {
        if (_queuedSeed == _currentSeed)
            return;

        if (_currentPhoto != null)
            Destroy(_currentPhoto);

        var sprite = GetComponent<SpriteRenderer>();

        _currentSeed = _queuedSeed;
        _currentPhoto = Visitor.BuildPseudoVisitor(_queuedSeed, sprite.sortingOrder, sprite.sortingLayerID, _queuedSpecies, LayerPrefab);
        _currentPhoto.transform.SetParent(transform, false);
    }
}
