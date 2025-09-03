using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _poolCapacity = 5;
    [SerializeField] protected int _poolMaxSize = 20;

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

    protected virtual T Get() => _pool.Get();

    protected virtual void Release(T obj) => _pool.Release(obj);
}