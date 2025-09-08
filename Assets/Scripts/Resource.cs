using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour, ITrackable<Resource>
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    public event Action<Resource> ReturnedToBase;
    public event Action<Resource> Collected;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SupplyCenter>(out _))
        {
            ReturnedToBase?.Invoke(this);
        }
    }

    public void Collect()
    {
        _collider.enabled = false;
        Collected?.Invoke(this);
    }

    public void ResetState()
    {
        _collider.enabled = true;
        transform.SetParent(null);
    }
}