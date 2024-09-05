using VContainer;
using VContainer.Unity;

public class LobbyInstaller : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Model, Presenter ���
        builder.Register<LobbyModel>(Lifetime.Singleton);
        builder.Register<LobbyPresenter>(Lifetime.Singleton);

        // View�� NetworkBehaviour�� LobbyInitializer ���
        builder.RegisterComponentInHierarchy<LobbyView>();
        builder.RegisterComponentInHierarchy<LobbyInitializer>();  // ���⼭ LobbyInitializer ���
    }
}