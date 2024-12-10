using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifeTimeScope : LifetimeScope
{
    [SerializeField] private StageDurationConfig stageDurationConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterGameComponents(builder);
        RegisterStateComponents(builder);
        RegisterStageComponents(builder);
        RegisterShopComponents(builder);
    }

    private void RegisterGameComponents(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<ChampionSpawner>();
        builder.RegisterComponentInHierarchy<GameStateManager>();
    }

    private void RegisterStateComponents(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<SelectObjectState>();
        builder.RegisterComponentInHierarchy<BattleReadyState>();
        builder.RegisterComponentInHierarchy<BattleState>();
        builder.RegisterComponentInHierarchy<WinState>();
        builder.RegisterInstance(stageDurationConfig);
        builder.RegisterComponentInHierarchy<SelectField>();
        builder.Register<StageStateBehaviour>(Lifetime.Singleton);
    }

    private void RegisterStageComponents(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<StagePresenter>();
        builder.RegisterComponentInHierarchy<StageView>();
        builder.RegisterComponentInHierarchy<StageModel>();
    }

    private void RegisterShopComponents(IContainerBuilder builder)
    {
        builder.Register<ShopPresenter>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<ShopModel>();
        builder.RegisterComponentInHierarchy<ShopView>();
    }
}
