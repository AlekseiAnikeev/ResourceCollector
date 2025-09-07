using System.Linq;
using ResourceCollector;
using UnityEngine;

public class HarvesterTracker : Tracker<Harvester>
{
    [SerializeField] private SupplyCenter _supplyCenter;

    public Harvester GetFreeHarvester()
    {
        return GetActiveObjects().FirstOrDefault(harvester => harvester.IsAvailable);
    }

    protected override void RegisterTrackableObject(Harvester obj)
    {
        obj.Init(_supplyCenter);
        base.RegisterTrackableObject(obj);
    }
}
