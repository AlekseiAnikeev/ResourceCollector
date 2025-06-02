using System;
using UnityEngine;

public class Resource2 : MonoBehaviour
{
    private Action<Resource2> _contact;

    public void Init(Action<Resource2> contact)
    {
        _contact = contact;
    }

    public void RemoveToPool()
    {
        _contact(this);
    }
}