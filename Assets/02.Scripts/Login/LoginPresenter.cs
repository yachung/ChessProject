using VContainer;
using VContainer.Unity;

public class LoginPresenter : IInitializable
{
    private readonly ILoginView view;
    private readonly LoginModel model;
    private readonly SceneLoader sceneLoader;

    public LoginPresenter(ILoginView view, LoginModel model, SceneLoader sceneLoader)
    {
        this.view = view;
        this.model = model;
        this.sceneLoader = sceneLoader;
    }

    public void Initialize()
    {
        view.OnLoginButtonClicked += Login;
        view.OnRegisterButtonClicked += Register;
    }

    private async void Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            view.ShowError("email or password is NullOrEmpty");
            return;
        }

        view.ShowLoading(true);

        var (result, errorMesage) = await model.AuthenticateUser(email, password);

        view.ShowLoading(false);

        if (result)
        {
            view.ShowSuccess($"{email} : 로그인 성공");
        }
        else
        {
            view.ShowError(errorMesage);
        }
    }

    private async void Register(string email, string password) 
    {
        view.ShowLoading(true);

        var (result, errorMesage) = await model.CreateAccount(email, password);

        view.ShowLoading(false);

        if (result)
        {
            view.ShowSuccess($"{email} : 회원가입 성공");
        }
        else
        {
            view.ShowError(errorMesage);
        }
    }
}
