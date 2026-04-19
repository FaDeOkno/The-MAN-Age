using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Visitor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public GameObject LayerPrefab;
    public GameEvent VisitorEnteredEvent;

    public Species Species;
    public int Age;
    public float DragToApprove = 1.5f;

    public bool IsValid { get; private set; }
    public bool InteractionAllowed { get; private set; } = false;

    private List<SpriteRenderer> _faceLayers = new();
    private bool _isDragging = false;
    private float _draggedDistance = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        Debug.Log($"Clicked on visitor with age {Age}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Dragged distance: {_draggedDistance}");
        _isDragging = false;

        var distance = _draggedDistance;

        _draggedDistance = 0f;

        if (!InteractionAllowed)
            return;

        if (distance < DragToApprove)
            return;

        Debug.Log("Approved visitor");
        GameManager.Instance.VisitorApprove();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!_isDragging || !InteractionAllowed)
            return;

        var delta = eventData.delta;
        _draggedDistance = Mathf.Clamp(_draggedDistance - (delta.x * .1f), -1, 100);
    }

    public void Generate(System.Random random, Species species)
    {
        foreach (var item in _faceLayers.Union(GetComponentsInChildren<SpriteRenderer>().Except(new[] { GetComponent<SpriteRenderer>() })))
        {
            Destroy(item.gameObject);
        }

        _faceLayers.Clear();

        if (random.Prob(.2f))
        {
            Age = random.Next(species.CommonAgeCap, species.RareAgeCap + 1);
        }
        else
        {
            Age = random.Next(species.MinAge, species.CommonAgeCap + 1);
        }

        IsValid = Age >= species.MatureAge;

        for (var i = 0; i < species.FaceLayers.Count; i++)
        {
            var sprite = random.Pick(species.FaceLayers[i].Sprites);
            var layer = Instantiate(LayerPrefab, transform);
            var renderer = layer.GetComponent<SpriteRenderer>();

            renderer.sprite = sprite;
            renderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + i + 1;
            _faceLayers.Add(renderer);
        }
    }

    public void PlaySpawnAnimation(Vector2 startPos, Vector2 endPos, TweenCallback onComplete = null)
    {
        transform.position = startPos;
        transform.DOMove(endPos, 1.2f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                InteractionAllowed = true;
                onComplete?.Invoke();
                VisitorEnteredEvent.Raise(this, null);
            });
    }

    public void FadeColor(float duration = 1.5f)
    {
        foreach (var layer in _faceLayers)
        {
            layer.DOColor(Color.black, duration).SetEase(Ease.InQuad);
        }
        GetComponent<SpriteRenderer>().DOColor(Color.black, duration).SetEase(Ease.InQuad);
    }
}
