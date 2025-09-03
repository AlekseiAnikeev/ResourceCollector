using ResourceCollector;
using UnityEngine;

public class HarvesterSpawner : Spawner<Harvester>
{
    [SerializeField] private int _initialCount = 3;
    [SerializeField] private float _spawnRadius = 3f;

    private void Start()
    {
        for (int i = 0; i < _initialCount; i++)
        {
            Harvester h = Get();
            Vector3 pos = transform.position + Random.insideUnitSphere * _spawnRadius;
            pos.y = 0;
            h.transform.position = pos;
        }
    }
    
    protected override Harvester CreateObject()
    {
        Harvester harvester = Instantiate(_prefab, transform);
        harvester.gameObject.SetActive(false);
        return harvester;
    }
}