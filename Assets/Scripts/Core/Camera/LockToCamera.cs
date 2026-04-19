using UnityEngine;

public class LockToCamera : MonoBehaviour
{
    private Vector3 _offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _offset = transform.position - Camera.main.transform.position;
    }

    void LateUpdate()
    {
        transform.position = Camera.main.transform.position + _offset;
    }
}
