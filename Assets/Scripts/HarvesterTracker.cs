using System.Linq;
using ResourceCollector;

public class HarvesterTracker : Tracker<Harvester>
{
    private SupplyCenter _supplyCenter;
    
    private void Awake()
    {
        _supplyCenter = GetComponentInParent<SupplyCenter>();
    }

    protected override void RegisterTrackableObject(Harvester obj)
    {
        obj.Init(_supplyCenter);
        base.RegisterTrackableObject(obj);
    }
    public Harvester GetFreeHarvester()
    {
        return GetActiveObjects().FirstOrDefault(harvester => harvester.IsAvailable);
    }
}
