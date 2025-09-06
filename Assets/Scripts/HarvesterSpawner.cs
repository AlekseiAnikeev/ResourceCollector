using ResourceCollector;
using UnityEngine;

public class HarvesterSpawner : Spawner<Harvester>
{
    [Range(0f, 15f)] [SerializeField] private int _initialCount = 3;

    private void Start()
    {
        for (int i = 0; i < _initialCount; i++)
        {
            Harvester harvester = Spawn();
            harvester.transform.position = GetSpawnPosition();
        }
    }

    protected override Harvester CreateObject()
    {
        Harvester harvester = Instantiate(_prefab, transform);
        harvester.gameObject.SetActive(false);
        return harvester;
    }
}