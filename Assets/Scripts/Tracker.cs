using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tracker<T> : MonoBehaviour where T : MonoBehaviour, ITrackable<T>
{
    private readonly List<T> _trackedObjects = new();

    public event Action<T> ObjectAdded;
    
    public List<T> GetActiveObjects() => new(_trackedObjects);

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out T obj))
            RegisterTrackableObject(obj);
    }

    protected virtual void OnDisable()
    {
        foreach (var obj in _trackedObjects)
            obj.Collected -= UnregisterTrackableObject;

        _trackedObjects.Clear();
    }

    protected virtual void RegisterTrackableObject(T obj)
    {
        if (_trackedObjects.Contains(obj))
            return;

        _trackedObjects.Add(obj);

        obj.Collected += UnregisterTrackableObject;
        ObjectAdded?.Invoke(obj);
    }

    private void UnregisterTrackableObject(T obj)
    {
        if (_trackedObjects.Remove(obj))
            obj.Collected -= UnregisterTrackableObject;
    }
}