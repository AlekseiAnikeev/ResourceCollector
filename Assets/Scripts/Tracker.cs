using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tracker<T> : MonoBehaviour where T : MonoBehaviour, ITrackable<T>
{
    private readonly List<T> _trackedObjects = new();

    public List<T> GetActiveObjects() => new(_trackedObjects);
    public event Action<T> OnObjectAdded;

    protected virtual void RegisterTrackableObject(T obj)
    {
        if (_trackedObjects.Contains(obj))
            return;

        _trackedObjects.Add(obj);

        obj.OnCollected += UnregisterTrackableObject;
        OnObjectAdded?.Invoke(obj);
    }

    private void UnregisterTrackableObject(T obj)
    {
        if (_trackedObjects.Remove(obj))
            obj.OnCollected -= UnregisterTrackableObject;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out T obj))
            RegisterTrackableObject(obj);
    }

    protected virtual void OnDisable()
    {
        foreach (var obj in _trackedObjects)
            obj.OnCollected -= UnregisterTrackableObject;

        _trackedObjects.Clear();
    }
}