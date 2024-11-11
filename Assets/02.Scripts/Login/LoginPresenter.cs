using VContainer;

public class LoginPresenter
{
    private ILoginView view;
    private LoginModel model;

    [Inject]
    public void Constructor(ILoginView view, LoginModel model)
    {
        this.view = view;
        this.model = model;

        view.OnLoginButtonClicked += Login;
        view.OnRegisterButtonClicked += Register;
    }

    private async void Login(string email, string password)
    {
        view.ShowLoading(true);

        bool success = await model.Login(email, password);

        view.ShowLoading(false);

        if (success)
        {
            view.ShowSuccess("view : 로그인 성공");
        }
        else
        {
            view.ShowSuccess("view : 로그인 실패");
        }
    }

    private async void Register(string email, string password) 
    {
        view.ShowLoading(true);

        bool success = await model.Register(email, password);

        view.ShowLoading(false);

        if (success)
        {
            view.ShowSuccess("view : 회원가입 성공");
        }
        else
        {
            view.ShowSuccess("view : 회원가입 실패");
        }
    }
}
