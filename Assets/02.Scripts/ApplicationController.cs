using VContainer;
using VContainer.Unity;
using UnityEngine;

public class ApplicationController : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SceneLoader>(Lifetime.Singleton);
        builder.Register<FirebaseManager>(Lifetime.Singleton);
    }
}
