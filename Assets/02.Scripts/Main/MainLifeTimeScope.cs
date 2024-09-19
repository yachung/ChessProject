using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifeTimeScope : LifetimeScope
{
    [SerializeField] private StageDurationConfig stageDurationConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<GameStateManager>();

        builder.RegisterComponentInHierarchy<SelectField>();

        builder.Register<StageStateBehaviour>(Lifetime.Scoped);
        builder.RegisterInstance(stageDurationConfig);

        builder.RegisterComponentInHierarchy<SelectObjectState>();
        builder.RegisterComponentInHierarchy<BattleReadyState>();
        builder.RegisterComponentInHierarchy<BattleState>();
        builder.RegisterComponentInHierarchy<WinState>();

        builder.RegisterComponentInHierarchy<StagePresenter>();
        builder.RegisterComponentInHierarchy<StageView>();
        builder.RegisterComponentInHierarchy<StageModel>();
    }
}
