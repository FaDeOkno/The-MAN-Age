using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Visitor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public GameObject LayerPrefab;
    public GameEvent VisitorEnteredEvent;

    public Species Species;
    public DateTime BirthDate;
    public string Name;

    public bool AlwaysValid = false;
    public bool HasId = true;

    public DialogueList[] HasIdDialogues;
    public DialogueList[] NoIdDialogues;
    public int Age => (int)((new DateTime(GameManager.Year, DateTime.Now.Month, DateTime.Now.Day) - BirthDate).TotalDays / 365.25);

    public int Seed;

    public float DragToApprove = 1.5f;

    public bool IsValid { get; private set; }
    public bool InteractionAllowed { get; private set; } = false;

    public List<SpriteRenderer> FaceLayers = new();
    private bool _isDragging = false;
    private float _draggedDistance = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        Debug.Log($"Birth date: {BirthDate}");
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

    public void Generate(int seed, Species species)
    {
        var random = new System.Random(seed);
        Seed = seed;
        Species = species;

        var date = new DateTime(GameManager.Year, DateTime.Now.Month, DateTime.Now.Day);

        // Clear old layers
        foreach (var item in FaceLayers.Union(GetComponentsInChildren<SpriteRenderer>().Except(new[] { GetComponent<SpriteRenderer>() })))
        {
            Destroy(item.gameObject);
        }

        FaceLayers.Clear();

        // Choose random dialogue
        if (new System.Random(seed + random.Next()).Prob(.2f))
        {
            HasId = false;
            GetComponent<DialogueCaller>().Dialogue = random.Pick(NoIdDialogues);
        }
        else
        {
            GetComponent<DialogueCaller>().Dialogue = random.Pick(HasIdDialogues);
        }

        // Generate and set the age
        if (random.Prob(.2f))
        {
            date = new DateTime(date.Year - random.Next(species.CommonAgeCap, species.RareAgeCap + 1), date.Month, date.Day);
        }
        else
        {
            date = new DateTime(date.Year - random.Next(species.MinAge, species.CommonAgeCap + 1), date.Month, date.Day);
        }

        date.AddDays(random.Next(365));
        BirthDate = date;

        IsValid = Age >= species.MatureAge && HasId;

        // Generate name
        Name = $"{random.Pick(Species.FirstNames)} {random.Pick(Species.LastNames)}";

        // Build the character
        for (var i = 0; i < species.FaceLayers.Count; i++)
        {
            var sprite = random.Pick(species.FaceLayers[i].Sprites);
            var layer = Instantiate(LayerPrefab, transform);
            layer.transform.localPosition = (Vector3)species.BodyOffset;
            var renderer = layer.GetComponent<SpriteRenderer>();

            renderer.sprite = sprite;
            renderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + i + 1;
            FaceLayers.Add(renderer);
        }
    }

    public void PlaySpawnAnimation(Vector2 startPos, Vector2 endPos, TweenCallback onComplete = null)
    {
        transform.position = startPos;
        transform.DOMove(endPos, 1.2f).SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                InteractionAllowed = true;
                onComplete?.Invoke();
                VisitorEnteredEvent.Raise(this, HasId);
                Debug.Log($"Visitor entered with age {Age} and seed {Seed}");
            });
    }

    public void FadeColor(float duration = 1.5f)
    {
        foreach (var layer in FaceLayers)
        {
            layer.DOColor(Color.black, duration).SetEase(Ease.InQuad);
        }
        GetComponent<SpriteRenderer>().DOColor(Color.black, duration).SetEase(Ease.InQuad);
    }

    public GameObject BuildPseudoVisitor(int renderDepth, int sortingLayer, GameObject layerPrefab)
    {
        var visitorObj = new GameObject("PseudoVisitor");
        SceneManager.MoveGameObjectToScene(visitorObj, SceneManager.GetActiveScene());

        for (var i = 0; i < FaceLayers.Count; i++)
        {
            var sprite = FaceLayers[i];
            var layer = Instantiate(layerPrefab, visitorObj.transform);
            var renderer = layer.GetComponent<SpriteRenderer>();

            renderer.sprite = sprite.sprite;
            renderer.sortingOrder = renderDepth + i + 1;
            renderer.sortingLayerID = sortingLayer;
        }

        return visitorObj;
    }
}
