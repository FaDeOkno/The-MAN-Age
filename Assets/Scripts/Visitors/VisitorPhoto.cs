using UnityEngine;

public class VisitorPhoto : MonoBehaviour
{
    [SerializeField] private GameObject LayerPrefab;

    private GameObject _currentPhoto;
    private Visitor _queuedVisitor;

    public void Generate(Component sender, object data)
    {
        if (data is not Visitor visitor)
        {
            Debug.LogError("Expected Visitor data for VisitorPhoto generation");
            return;
        }

        Debug.Log($"Generating photo for visitor with seed {visitor.Seed} and species {visitor.Species}");

        _queuedVisitor = visitor;

        if (_currentPhoto != null)
            Destroy(_currentPhoto);

        var sprite = GetComponent<SpriteRenderer>();

        _currentPhoto = visitor.BuildPseudoVisitor(sprite.sortingOrder, sprite.sortingLayerID, LayerPrefab);
        _currentPhoto.transform.SetParent(transform, false);
    }

    public void QueueMove()
    {
    }
}
