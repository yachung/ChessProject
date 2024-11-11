using VContainer;
using VContainer.Unity;
using UnityEngine;

public class RootLifeTimeScope : LifetimeScope
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SceneLoader>(Lifetime.Singleton);
        builder.Register<FirebaseManager>(Lifetime.Singleton);
        //builder.Register<AudioManager>(Lifetime.Singleton);
    }
}
