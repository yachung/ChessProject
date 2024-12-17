using System;
using UnityEngine;
using VContainer.Unity;

public abstract class BaseEntryPoint : IStartable, IDisposable
{
    public virtual void Start()
    {
    }

    public virtual void Dispose()
    {
    }
}
