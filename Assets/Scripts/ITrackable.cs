using System;

public interface ITrackable<T>
{
   event Action<T> OnCollected;
}
