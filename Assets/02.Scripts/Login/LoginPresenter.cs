using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class LoginPresenter : IInitializable
{
    [Inject] private readonly UIManager uiManager;
    [Inject] private readonly SceneLoader sceneLoader;      // Fusion2의 sceneLoad를 사용하지 않으므로 필요하지 않을 수 있음.

    private readonly ILoginView view;
    private readonly LoginModel model;

    public LoginPresenter(ILoginView view, LoginModel model)
    {
        this.view = view;
        this.model = model;
    }

    public void Initialize()
    {
        view.OnLoginButtonClicked += Login_Email;
        view.OnRegisterButtonClicked += Register;
        view.OnGoogleLoginButtonClicked += Login_Google;
        view.OnGuestLoginButtonClicked += Login_Guest;
    }

    private async void HandleAuthenticate(Func<UniTask<(bool, string)>> loginFunc, string successMessage, SceneType sceneToLoad = SceneType.None)
    {
        uiManager.ShowLoading(true);

        var (result, errorMessage) = await loginFunc();

        uiManager.ShowLoading(false);

        if (result)
        {
            uiManager.ShowMessage(successMessage);
            if (sceneToLoad != SceneType.None)
            {
                SceneManager.LoadScene((int)sceneToLoad);
                //sceneLoader.LoadScene(sceneToLoad);
            }
        }
        else
        {
            uiManager.ShowMessage(errorMessage);
        }
    }

    private void Login_Email(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            uiManager.ShowMessage("email or password Field is NullOrEmpty");
            return;
        }

        HandleAuthenticate(() => model.EmailAuthenticateResult(email, password), $"{email} : 로그인 성공", SceneType.Lobby);
    }

    private void Register(string email, string password) 
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            uiManager.ShowMessage("email or password Field is NullOrEmpty");
            return;
        }

        HandleAuthenticate(() => model.CreateAccountAuthenticateResult(email, password), $"{email} : 회원가입 성공");
    }

    private void Login_Google()
    {
        HandleAuthenticate(() => model.GoogleAuthenticateResult(), $"구글 로그인 성공", SceneType.Lobby);
    }

    private void Login_Guest()
    {
        HandleAuthenticate(() => model.GuestAuthenticateResult(), $"게스트 로그인 성공", SceneType.Lobby);
    }
}
