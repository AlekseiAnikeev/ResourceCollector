using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Resource2 _prefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 20;
    [SerializeField] private float _spawnInterval = 10f;
    [SerializeField] private float _spawnRadius = 50f;

    private ObjectPool<Resource2> _pool;
    private List<Resource2> list = new();
    private Coroutine _coroutine;

    private void Awake()
    {
        _pool = new ObjectPool<Resource2>(
            createFunc: Create,
            actionOnGet: prefab => prefab.gameObject.SetActive(true),
            actionOnRelease: prefab => prefab.gameObject.SetActive(false),
            actionOnDestroy: prefab => Destroy(prefab.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    private void Start()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Countdown(_spawnInterval));
    }

    private void Spawn()
    {
        Resource2 resource = GetObject();
        resource.transform.position = GetRandomPosition();
        resource.Init(RemoveToPool);
        list.Add(resource);
    }

    private Resource2 GetObject()
    {
        Resource2 lol = _pool.Get();
        list.Add(lol);
        return lol;
    }

    private void RemoveToPool(Resource2 obj)
    {
        list.Remove(obj);
        _pool.Release(obj);
    }

    private Resource2 Create()
    {
        return Instantiate(_prefab);
    }
    private IEnumerator Countdown(float delay)
    {
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            Spawn();

            yield return wait;
        }
    }

    public List<Resource2>GetActiveResources() => list;
    
    private Vector3 GetRandomPosition()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * _spawnRadius;
        pos.y = 0;
        return pos;
    }
}