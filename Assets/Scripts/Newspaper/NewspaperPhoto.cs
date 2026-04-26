using UnityEngine;
using UnityEngine.SceneManagement;

public class NewspaperPhoto : MonoBehaviour
{
    [SerializeField] private GameObject _layerPrefab;

    private Transform _current;

    public void OnEvent(Component sender, object data)
    {
        if (data is not DayData day)
            return;

        if (_current != null)
            Destroy(_current);

        var sprite = GetComponent<SpriteRenderer>();
        var face = day.NewsFacePrefab.GetComponent<Visitor>().BuildPseudoVisitor(sprite.sortingOrder, sprite.sortingLayerID, _layerPrefab);
        SceneManager.MoveGameObjectToScene(face.gameObject, SceneManager.GetActiveScene());
        face.transform.SetParent(transform, false);
        _current = face.transform;
    }
}
