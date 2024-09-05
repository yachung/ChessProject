using VContainer;
using VContainer.Unity;

public class LobbyInstaller : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Model, Presenter 등록
        builder.Register<LobbyModel>(Lifetime.Singleton);
        builder.Register<LobbyPresenter>(Lifetime.Singleton);

        // View와 NetworkBehaviour인 LobbyInitializer 등록
        builder.RegisterComponentInHierarchy<LobbyView>();
        builder.RegisterComponentInHierarchy<LobbyInitializer>();  // 여기서 LobbyInitializer 등록
    }
}