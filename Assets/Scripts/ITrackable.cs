using System;

public interface ITrackable<T>
{
   public event Action<T> Collected;
}
