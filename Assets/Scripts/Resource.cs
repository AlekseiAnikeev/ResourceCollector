using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour, ITrackable<Resource>
{
    public event Action<Resource> OnReturnedToBase;
    public event Action<Resource> OnCollected;

    public bool IsCollected
    {
        get => _isCollected;
        private set
        {
            if (_isCollected == value)
                return;

            _isCollected = value;

            if (_isCollected && IsTargeted)
                OnCollected?.Invoke(this);
        }
    }

    public bool IsTargeted { get; private set; }

    private Rigidbody _rigidbody;
    private bool _isCollected;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    public void SetTargeted() => IsTargeted = true;

    public void SetCollected() => IsCollected = true;

    public void ResetState()
    {
        IsCollected = false;
        IsTargeted = false;
        transform.SetParent(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SupplyCenter>(out _))
        {
            OnReturnedToBase?.Invoke(this);
        }
    }
}