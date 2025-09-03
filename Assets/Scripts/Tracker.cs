using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tracker<T> : MonoBehaviour where T : MonoBehaviour, ITrackable<T>
{
    [SerializeField] private float _radius = 15f;
    [SerializeField] private float _updateInterval = 0.2f;

    private readonly List<T> _objects = new();
    public event Action<T> OnObjectAdded;
    private float _timer;
    public List<T> GetActiveObjects() => new (_objects);

    protected virtual void Register(T obj)
    {
        if (_objects.Contains(obj)) 
            return;
        _objects.Add(obj);
        obj.Collected += Unregister;
        OnObjectAdded?.Invoke(obj);
    }

    private void Unregister(T obj)
    {
        if (_objects.Remove(obj))
        {
            obj.Collected -= Unregister;
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out T obj))
        {
            Register(obj);
        }
    }

    protected virtual void OnDisable()
    {
        foreach (var obj in _objects)
        {
            obj.Collected -= Unregister;
        }
        _objects.Clear();
    }
}