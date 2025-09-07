using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour, ITrackable<Resource>
{
    private Rigidbody _rigidbody;
    private bool _isCollected;
    
    public event Action<Resource> ReturnedToBase;
    public event Action<Resource> Collected;
    
    public bool IsCollected
    {
        get => _isCollected;
        private set
        {
            if (_isCollected == value)
                return;

            _isCollected = value;

            if (_isCollected && IsTargeted)
                Collected?.Invoke(this);
        }
    }

    public bool IsTargeted { get; private set; }

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

    public void SetTargeted() =>
        IsTargeted = true;

    public void SetCollected() => 
        IsCollected = true;

    public void ResetState()
    {
        IsCollected = false;
        IsTargeted = false;
        transform.SetParent(null);
    }
}