using VContainer;
using VContainer.Unity;

public class LoginPresenter : IInitializable
{
    [Inject] private readonly UIManager uiManager;
    [Inject] private readonly SceneLoader sceneLoader;

    private readonly ILoginView view;
    private readonly LoginModel model;

    public LoginPresenter(ILoginView view, LoginModel model)
    {
        this.view = view;
        this.model = model;
    }

    public void Initialize()
    {
        view.OnLoginButtonClicked += Login;
        view.OnRegisterButtonClicked += Register;
        view.OnGoogleLoginButtonClicked += GoogleLoginAsync;
    }

    private async void Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            uiManager.ShowMessage("email or password is NullOrEmpty");
            return;
        }

        uiManager.ShowLoading(true);

        var (result, errorMesage) = await model.AuthenticateUser(email, password);

        uiManager.ShowLoading(false);

        if (result)
        {
            uiManager.ShowMessage($"{email} : 로그인 성공");
            sceneLoader.LoadScene(SceneType.Lobby);
        }
        else
        {
            uiManager.ShowMessage(errorMesage);
        }
    }

    private async void Register(string email, string password) 
    {
        uiManager.ShowLoading(true);

        var (result, errorMessage) = await model.CreateAccount(email, password);

        uiManager.ShowLoading(false);

        if (result)
        {
            uiManager.ShowMessage($"{email} : 회원가입 성공");
        }
        else
        {
            uiManager.ShowMessage(errorMessage);
        }
    }

    private async void GoogleLoginAsync()
    {
        uiManager.ShowLoading(true);

        var (result, errorMessage) = await model.AuthenticateUser();

        uiManager.ShowLoading(false);

        if (result)
        {
            uiManager.ShowMessage($"구글 로그인 성공");
            sceneLoader.LoadScene(SceneType.Lobby);
        }
        else
        {
            uiManager.ShowMessage(errorMessage);
        }
    }
}
