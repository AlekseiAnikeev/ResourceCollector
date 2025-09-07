using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour, ITrackable<Resource>
{
    private Rigidbody _rigidbody;

    public event Action<Resource> ReturnedToBase;
    public event Action<Resource> Collected;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SupplyCenter>(out _))
        {
            ReturnedToBase?.Invoke(this);
        }
    }

    public void SetCollected()
    {
        Collected?.Invoke(this);
    }

    public void ResetState()
    {
        transform.SetParent(null);
    }
}