using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Visitor : MonoBehaviour
{
    public GameObject LayerPrefab;
    public Species Species;
    public int Age;
    public float DragToApprove = 1.5f;

    public bool IsValid { get; private set; }
    public bool InteractionAllowed { get; private set; } = false;

    private List<SpriteRenderer> _faceLayers = new();
    private bool _isDragging = false;
    private float _draggedDistance = 0f;

    void OnMouseDown()
    {
        _isDragging = true;
        Debug.Log($"Clicked on visitor with age {Age}");
    }

    void OnMouseUp()
    {
        _isDragging = false;
        _draggedDistance = 0f;

        if (!InteractionAllowed)
            return;

        if (_draggedDistance < DragToApprove)
            return;

        Debug.Log("Approved visitor");
        GameManager.Instance.VisitorApprove();
    }

    public void GetMouseDelta(InputAction.CallbackContext context)
    {
        if (!_isDragging || !InteractionAllowed)
            return;

        var delta = context.ReadValue<Vector2>();
        _draggedDistance = Mathf.Clamp(_draggedDistance - delta.x, -100, 1);
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

    public void PlaySpawnAnimation(Vector2 startPos, Vector2 endPos)
    {
        transform.position = startPos;
        transform.DOMove(endPos, 1.2f).SetEase(Ease.OutBack).OnComplete(() => InteractionAllowed = true);
    }
}
