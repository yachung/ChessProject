using VContainer;
using VContainer.Unity;

public class LobbyLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<LobbyEntryPoint>();

        // Model, View 등록
        builder.RegisterComponentInHierarchy<RoomModel>();
        builder.RegisterComponentInHierarchy<RoomView>();

        // Presenter 등록
        builder.RegisterComponentInHierarchy<RoomPresenter>();

        builder.RegisterComponentInHierarchy<LobbyController>();

        //builder.RegisterComponentInHierarchy<PregameState>();
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