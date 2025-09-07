using System;
using System.Collections;
using ResourceCollector;
using UnityEngine;

public class SupplyCenter : MonoBehaviour
{
    [Range(0f, 10f)] [SerializeField] private float _resourceScanInterval = 5f;
    [SerializeField] private AudioClip _deliverySound;
    [SerializeField] private ParticleSystem _deliveryParticles;
    [SerializeField] private ResourceTracker _resourceTracker;
    [SerializeField] private HarvesterTracker _harvesterTracker;

    private Coroutine _scanningCoroutine;
    private WaitForSeconds _scanDelay;
    private int _storedResources;

    public event Action<int> ResourcesCountChanged;

    private void Awake()
    {
        _scanDelay = new WaitForSeconds(_resourceScanInterval);
    }

    private void Start()
    {
        if (_scanningCoroutine != null)
            StopCoroutine(_scanningCoroutine);

        _scanningCoroutine = StartCoroutine(ScanResourcesCoroutine());
    }

    private void OnEnable()
    {
        _harvesterTracker.ObjectAdded += OnObjectAdded;
    }

    private void OnDisable()
    {
        _harvesterTracker.ObjectAdded -= OnObjectAdded;
    }

    private void OnDestroy()
    {
        if (_scanningCoroutine != null)
            StopCoroutine(_scanningCoroutine);
    }

    private void OnObjectAdded(Harvester harvester)
    {
        harvester.ResourceDelivered += OnResourceDelivered;
    }

    private void OnResourceDelivered(Resource resource)
    {
        _storedResources++;
        ResourcesCountChanged?.Invoke(_storedResources);

        resource.ResetState();
        _resourceTracker.Release(resource);

        if (_deliverySound != null)
            AudioSource.PlayClipAtPoint(_deliverySound, transform.position);

        _deliveryParticles?.Play();
    }

    private IEnumerator ScanResourcesCoroutine()
    {
        while (true)
        {
            yield return _scanDelay;
            AssignAvailableResources();
        }
    }

    private void AssignAvailableResources()
    {
        foreach (var resource in _resourceTracker.GetAvailableObjects())
        {
            Harvester freeHarvester = _harvesterTracker.GetFreeHarvester();

            if (freeHarvester == null)
                break;

            if (_resourceTracker.TrySetIsTarget(resource))
            {
                freeHarvester.Collect(resource);
            }
        }
    }
}