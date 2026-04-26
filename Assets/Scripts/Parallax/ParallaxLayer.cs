using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float _parallaxFactor;
    private Vector3 _lastCameraPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lastCameraPosition = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        transform.localPosition += (Camera.main.transform.position - _lastCameraPosition) * _parallaxFactor;
        _lastCameraPosition = Camera.main.transform.position;
    }
}
