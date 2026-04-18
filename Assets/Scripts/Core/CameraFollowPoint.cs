using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowPoint : MonoBehaviour
{
    [SerializeField] private float _limits = .4f;

    // Update is called once per frame
    void Update()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var cam = Camera.main;

        var mouseWorldPos = (Vector2)(cam.ScreenToWorldPoint(mousePos) - cam.transform.position);
        transform.position = Vector2.ClampMagnitude(mouseWorldPos, _limits);
    }
}
