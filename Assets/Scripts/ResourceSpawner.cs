using UnityEngine;
using System.Collections.Generic;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float _spawnRadius = 50f;
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private int _poolSize = 20;
    
    [Header("Зависимости")]
    [SerializeField] private GameObject _resourcePrefab;
    
    private readonly Queue<Resource> _resourcePool = new();
    private readonly List<Resource> _activeResources = new();

    private void Start()
    {
        InitializePool();
        InvokeRepeating(nameof(SpawnResource), 0f, _spawnInterval);
    }
    
    public List<Resource> GetActiveResources() => _activeResources;

    public void ReturnResource(Resource resource)
    {
        if (!_activeResources.Contains(resource)) return;
        resource.FullReset();
        resource.gameObject.SetActive(false);
        _activeResources.Remove(resource);
        _resourcePool.Enqueue(resource);
    }
    
    private void InitializePool()
    {
        for (var i = 0; i < _poolSize; i++)
        {
            var obj = Instantiate(_resourcePrefab, Vector3.zero, Quaternion.identity);
            var resource = obj.GetComponent<Resource>();
            obj.SetActive(false);
            _resourcePool.Enqueue(resource);
        }
    }

    private void SpawnResource()
    {
        if (_resourcePool.Count == 0) return;

        Resource resource = _resourcePool.Dequeue();
        resource.FullReset();
        resource.transform.position = GetRandomPosition();
        resource.gameObject.SetActive(true);
        _activeResources.Add(resource);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * _spawnRadius;
        pos.y = 0;
        return pos;
    }
}