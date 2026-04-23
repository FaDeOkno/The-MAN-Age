using Unity.Cinemachine;
using UnityEngine;

public class CameraPointSwitch : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;
    private float _sizeChangeSpeed = 5f;
    private float _originalSize;
    private float _targetSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _originalSize = _cinemachineCamera.Lens.OrthographicSize;
        _targetSize = _cinemachineCamera.Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cinemachineCamera.Lens.OrthographicSize != _targetSize)
            _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCamera.Lens.OrthographicSize, _targetSize, _sizeChangeSpeed * Time.deltaTime);
    }

    public void OnEvent(Component sender, object data)
    {
        if (data is not Transform target)
            return;

        if (sender is not ClickableId clickableId)
            return;

        _cinemachineCamera.Follow = target;
        _targetSize = clickableId.IsActive ? _originalSize * clickableId.ScaleMultiplier : _originalSize;
    }
}
