public class ResourceDatabase<T>
{
    private T _resource;

    public bool IsTargeted { get; private set; }

    public ResourceDatabase(T obj)
    {
        _resource = obj;
        IsTargeted = false;
    }
    
    public void SetTargeted() =>
        IsTargeted = true;
}
