using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [Range(0f, 10f)] [SerializeField] private float _spawnInterval = 2f;
    
    private Coroutine _spawnCoroutine;

    protected void Start()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
        
        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private void OnDestroy()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    protected override Resource CreateObject()
    {
        Resource resource = Instantiate(_prefab);
        resource.gameObject.SetActive(false);
        return resource;
    }

    protected override Resource Spawn()
    {
        Resource resource = base.Spawn();
        resource.OnReturnedToBase += ReturnToPool;
        resource.transform.position = GetSpawnPosition();
        return resource;
    }

    private IEnumerator SpawnCoroutine()
    {
        var wait = new WaitForSeconds(_spawnInterval);
        while (enabled)
        {
            Spawn();
            yield return wait;
        }
    }

    private void ReturnToPool(Resource resource)
    {
        resource.OnReturnedToBase -= ReturnToPool;
        resource.ResetState();
        Release(resource);
    }
}