using System;
using UnityEngine;
using VContainer.Unity;

public abstract class BaseEntryPoint : IStartable, IDisposable
{
    public virtual void Start()
    {
        Debug.Log($"BaseEntryPoint started.");
    }

    public virtual void Dispose()
    {
        Debug.Log("BaseEntryPoint disposed.");
    }
}
