using UnityEngine;

public class ResourceTracker : Tracker<Resource>
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && resource.IsCollected == false)
            RegisterTrackableObject(resource);
    }
}