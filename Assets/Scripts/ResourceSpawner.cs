using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [Range(0f, 10f)] [SerializeField] private float _spawnInterval = 2f;
    [Range(0f, 10f)] [SerializeField] private float _spawnRadius = 5f;

    private Coroutine _coroutine;

    protected void Start()
    {
        _coroutine = StartCoroutine(SpawnRoutine());
    }
    private new Resource Get()
    {
        Resource r = base.Get();
        r.ReturnedToBase += RemoveToPool;
        return r;
    }

    protected override Resource CreateObject()
    {
        Resource resource = Instantiate(_prefab);
        resource.gameObject.SetActive(false);
        return resource;
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(_spawnInterval);
        while (enabled)
        {
            Spawn();
            yield return wait;
        }
    }

    private void Spawn()
    {
        Resource r = Get();
        r.transform.position = GetRandomPosition();
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * _spawnRadius;
        pos.y = 0;
        return pos;
    }

    private void RemoveToPool(Resource resource)
    {
        resource.ReturnedToBase -= RemoveToPool;
        resource.ResetState();
        Release(resource);
    }
}