using VContainer;
using VContainer.Unity;

public class LoginLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Model, Presenter 등록
        builder.Register<LoginModel>(Lifetime.Scoped);
        builder.Register<LoginPresenter>(Lifetime.Scoped);

        // View 등록
        builder.RegisterComponentInHierarchy<LoginView>();
    }
}