using System.Linq;
using ResourceCollector;

public class HarvesterTracker : Tracker<Harvester>
{
    private SupplyCenter _center;
    
    private void Awake()
    {
        _center = GetComponentInParent<SupplyCenter>();
    }

    protected override void Register(Harvester obj)
    {
        obj.Init(_center);
        base.Register(obj);
    }
    public Harvester GetFreeHarvester()
    {
        return GetActiveObjects().FirstOrDefault(harvester => harvester.IsAvailable);
    }
}
