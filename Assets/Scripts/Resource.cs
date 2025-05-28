using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsCollected { get; private set; }
    public bool IsTargeted { get; private set; }

    public void MarkAsTargeted() => IsTargeted = true;

    public void FullReset()
    {
        IsCollected = false;
        IsTargeted = false;
        transform.SetParent(null);
        transform.position = Vector3.zero;
    }
}