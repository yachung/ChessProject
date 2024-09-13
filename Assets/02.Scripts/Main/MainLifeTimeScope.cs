using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<GameStateManager>();

        builder.RegisterComponentInHierarchy<SelectField>();

        builder.RegisterComponentInHierarchy<SelectObjectState>();
        builder.RegisterComponentInHierarchy<BattleReadyState>();
        builder.RegisterComponentInHierarchy<BattleState>();
        builder.RegisterComponentInHierarchy<WinState>();

        builder.RegisterComponentInHierarchy<GameView>();
    }
}
