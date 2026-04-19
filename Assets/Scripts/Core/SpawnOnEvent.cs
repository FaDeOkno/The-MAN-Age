using UnityEngine;

public class SpawnOnEvent : MonoBehaviour
{
    [SerializeField] private GameObject _spawnedPrefab;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private bool _removeOnSpawn = false;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _offset, 0.5f);
    }

    public void Spawn(Component sender, object param)
    {
        Spawn();
    }

    public void Spawn()
    {
        var obj = Instantiate(_spawnedPrefab, transform.parent);

        obj.transform.position = transform.position + _offset;

        if (_removeOnSpawn)
        {
            Destroy(this);
        }
    }
}
