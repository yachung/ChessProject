using Cysharp.Threading.Tasks;
using Firebase.Auth;
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
        view.OnGoogleLoginButtonClicked += Login_Google;
        view.OnGuestLoginButtonClicked += Login_Guest;

        view.OnRegisterButtonClicked += Register;
    }

    private async void HandleLoginAuthenticate(Func<UniTask<(bool, string)>> LoginAuthenticateFunc, string successMessage)
    {
        uiManager.ShowLoading(true);

        var (result, errorMessage) = await LoginAuthenticateFunc();

        uiManager.ShowLoading(false);

        if (result)
        {
            if (string.IsNullOrEmpty(model.firebaseUser.DisplayName))
            {
                string displayName = null;

                // view.OnInputNickName => displayName = value;

                uiManager.ShowLoading(true);

                (result, errorMessage) = await model.SetProfileAuthenticateResult(displayName);

                uiManager.ShowLoading(false);
            }
        }

        if (result)
        {
            uiManager.ShowMessage($"{model.firebaseUser.DisplayName}\n{successMessage}");

            SceneManager.LoadScene((int)SceneType.Lobby);
        }
        else
        {
            uiManager.ShowMessage(errorMessage);
        }
    }

    private async void HandleRegistration(Func<UniTask<(bool, string)>> registrationFunc, string successMessage)
    {
        uiManager.ShowLoading(true);

        var (result, errorMessage) = await registrationFunc();

        uiManager.ShowLoading(false);

        if (result)
        {
            uiManager.ShowMessage(successMessage);
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

        HandleLoginAuthenticate(() => model.EmailAuthenticateResult(email, password), $"{email} : 로그인 성공");
    }

    private void Login_Google()
    {
        HandleLoginAuthenticate(() => model.GoogleAuthenticateResult(), $"구글 로그인 성공");
    }

    private void Login_Guest()
    {
        HandleLoginAuthenticate(() => model.GuestAuthenticateResult(), $"게스트 로그인 성공");
    }

    private void Register(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            uiManager.ShowMessage("email or password Field is NullOrEmpty");
            return;
        }

        HandleRegistration(() => model.CreateAccountAuthenticateResult(email, password), $"{email} : 회원가입 성공");
    }
}
