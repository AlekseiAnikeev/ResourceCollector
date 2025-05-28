using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SupplyCenter : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private int _initialUnits = 3;
    [SerializeField] private float _scanInterval = 5f;
    [SerializeField] private float _spawnRadius = 3f;

    [Header("Зависимости")]
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private ResourceSpawner _resourceSpawner;

    [Header("Эффекты")]
    [SerializeField] private AudioClip _deliverySound;
    [SerializeField] private ParticleSystem _deliveryParticles;

    public event Action<int> OnResourcesChanged;

    private readonly List<Harvester> _allUnits = new();
    private Coroutine _scanCoroutine;
    private int _availableResources;

    private void Start()
    {
        Initialize();
        
        if (_scanCoroutine != null) 
            StopCoroutine(_scanCoroutine);
        
        _scanCoroutine = StartCoroutine(ScanRoutine());
    }

    private void OnDestroy()
    {
        if (_scanCoroutine != null) 
            StopCoroutine(_scanCoroutine);
        
        foreach (var unit in _allUnits) unit.OnResourceDelivered -= HandleResourceDelivery;
    }

    private void Initialize()
    {
        for (var i = 0; i < _initialUnits; i++) SpawnUnit();
    }

    private IEnumerator ScanRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_scanInterval);
            ScanForResources();
        }
    }

    private void HandleResourceDelivery(Resource resource)
    {
        _availableResources++;
        OnResourcesChanged?.Invoke(_availableResources);
        AudioSource.PlayClipAtPoint(_deliverySound, transform.position);
        _deliveryParticles?.Play();
        _resourceSpawner.ReturnResource(resource);
    }

    private void SpawnUnit()
    {
        Vector3 spawnPos = transform.position + Random.insideUnitSphere * _spawnRadius;
        spawnPos.y = 0;
        
        Harvester unit = Instantiate(_unitPrefab, spawnPos, Quaternion.identity).GetComponent<Harvester>();
        unit.Init(this);
        
        unit.OnResourceDelivered += HandleResourceDelivery;
        
        _allUnits.Add(unit);
    }

    private void ScanForResources()
    {
        foreach (var resource in _resourceSpawner.GetActiveResources())
        {
            if (resource.IsCollected || resource.IsTargeted)
                continue;
            
            var freeUnit = _allUnits.Find(u => u.IsAvailable);
            if (freeUnit == null) 
                break;
            freeUnit.AssignResource(resource);
        }
    }
} 