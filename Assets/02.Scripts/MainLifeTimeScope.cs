using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameState>();
    }
}
