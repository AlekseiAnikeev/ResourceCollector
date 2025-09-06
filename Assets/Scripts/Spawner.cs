using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [Range(0f, 10f)] [SerializeField] protected float _spawnRadius = 5f;
    [Range(0f, 20f)] [SerializeField] protected int _poolCapacity = 5;
    [Range(0f, 50f)] [SerializeField] protected int _poolMaxSize = 20;

    private ObjectPool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreateObject,
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    protected abstract T CreateObject();

    protected virtual T Spawn() => _pool.Get();

    protected void Release(T obj) => _pool.Release(obj);

    protected Vector3 GetSpawnPosition()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * _spawnRadius;
        pos.y = 0;
        return pos;
    }
}