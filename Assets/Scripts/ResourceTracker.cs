using UnityEngine;

public class ResourceTracker : Tracker<Resource>
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource obj))
        {
            if (obj.IsCollected == false)
                Register(obj);
        }
    }
}