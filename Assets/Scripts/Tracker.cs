using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tracker<T> : MonoBehaviour where T : MonoBehaviour, ITrackable<T>
{
    private readonly Dictionary<T, bool> _trackedObjects = new();

    public event Action<T> ObjectAdded;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out T obj))
            RegisterTrackableObject(obj);
    }

    protected virtual void OnDisable()
    {
        foreach (var trackedObject in _trackedObjects)
        {
            if (trackedObject.Key is ITrackable<T> trackable)
                trackable.Collected -= UnregisterTrackableObject;
        }

        _trackedObjects.Clear();
    }

    public List<T> GetAvailableObjects() =>
        (from obj in _trackedObjects where obj.Value == false select obj.Key).ToList();

    public bool TrySetIsTarget(T obj)
    {
        if (_trackedObjects.ContainsKey(obj) == false || _trackedObjects[obj])
            return false;

        _trackedObjects[obj] = true;
        return true;
    }

    public void Release(T obj)
    {
        if (_trackedObjects.ContainsKey(obj))
            _trackedObjects[obj] = false;
    }

    protected List<T> GetActiveObjects() =>
        new(_trackedObjects.Keys);

    protected virtual void RegisterTrackableObject(T obj)
    {
        if (_trackedObjects.ContainsKey(obj))
            return;

        obj.Collected += UnregisterTrackableObject;

        _trackedObjects[obj] = false;

        ObjectAdded?.Invoke(obj);
    }

    private void UnregisterTrackableObject(T obj)
    {
        if (_trackedObjects.Remove(obj))
        {
            obj.Collected -= UnregisterTrackableObject;
        }
    }
}