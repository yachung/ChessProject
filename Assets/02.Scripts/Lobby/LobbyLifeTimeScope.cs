using VContainer;
using VContainer.Unity;

public class LobbyLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Model, View 등록
        builder.RegisterComponentInHierarchy<LobbyModel>();
        builder.RegisterComponentInHierarchy<LobbyView>();

        // Presenter 등록
        builder.RegisterComponentInHierarchy<LobbyPresenter>();

        builder.RegisterComponentInHierarchy<PregameState>();
        // State 등록, Entry Point
        //builder.RegisterComponentInHierarchy<PregameState>();
    }

    //private void Start()
    //{
    //    Container.Resolve<LobbyModel>();
    //    Container.Resolve<LobbyView>();
    //    Container.Resolve<LobbyPresenter>();
    //}
}