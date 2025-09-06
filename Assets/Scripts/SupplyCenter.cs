using System;
using System.Collections;
using ResourceCollector;
using UnityEngine;

public class SupplyCenter : MonoBehaviour
{
    [Range(0f, 10f)] [SerializeField] private float _resourceScanInterval = 5f;

    [SerializeField] private AudioClip _deliverySound;
    [SerializeField] private ParticleSystem _deliveryParticles;

    private ResourceTracker _resourceTracker;
    private HarvesterTracker _harvesterTracker;

    public event Action<int> ResourcesCountChanged;

    private Coroutine _scanningCoroutine;
    private WaitForSeconds _scanDelay;
    private int _storedResources;

    private void Awake()
    {
        _resourceTracker = GetComponentInChildren<ResourceTracker>();
        _harvesterTracker = GetComponentInChildren<HarvesterTracker>();

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
        _harvesterTracker.OnObjectAdded += HarvesterAdded;
    }

    private void OnDisable()
    {
        _harvesterTracker.OnObjectAdded -= HarvesterAdded;
    }

    private void OnDestroy()
    {
        if (_scanningCoroutine != null)
            StopCoroutine(_scanningCoroutine);
    }

    private void HarvesterAdded(Harvester harvester)
    {
        harvester.OnResourceDelivered += ResourceDelivered;
    }

    private void ResourceDelivered(Resource resource)
    {
        _storedResources++;
        ResourcesCountChanged?.Invoke(_storedResources);

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
        foreach (var resource in _resourceTracker.GetActiveObjects())
        {
            if (resource.IsCollected || resource.IsTargeted)
                continue;

            Harvester freeHarvester = _harvesterTracker.GetFreeHarvester();
            
            if (freeHarvester == null)
                break;

            freeHarvester.Collect(resource);
        }
    }
}