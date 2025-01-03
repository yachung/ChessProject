using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class LoginPresenter : IInitializable
{
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

        // 닉네임 입력 완료 시점을 Presenter가 받을 수 있도록 연결
        view.OnNicknameEntered += OnNicknameEntered;
    }

    private async void HandleLoginAuthenticate(Func<UniTask<(bool, string)>> LoginAuthenticateFunc, string successMessage)
    {
        view.ShowLoading(true);

        var (result, errorMessage) = await LoginAuthenticateFunc();

        view.ShowLoading(false);

        // 로그인 실패 시
        if (!result)
        {
            view.ShowMessage(errorMessage);
            return;
        }

        // 로그인 성공이면 DisplayName 확인
        if (string.IsNullOrEmpty(model.firebaseUser.DisplayName))
        {
            // 1) 닉네임이 필요하다는 사실만 인지
            // 2) View에게 "닉네임 입력" 요청 → 팝업 열기 등은 View가 알아서 처리
            view.RequestNicknameInput();
            // 닉네임 입력이 끝나면 View가 OnNicknameEntered 이벤트로 Presenter에게 알려줄 것
        }
        else
        {
            // 이미 닉네임이 있는 경우 곧바로 로비 이동
            ProceedToLobby(successMessage);
        }
    }

    /// <summary>
    /// View 쪽에서 닉네임 입력이 끝나면 Presenter로 전달받는 콜백
    /// </summary>
    private async void OnNicknameEntered(string nickname)
    {
        view.ShowLoading(true);
        var (result, errorMessage) = await model.SetProfileAuthenticateResult(nickname);
        view.ShowLoading(false);

        if (!result)
        {
            // 닉네임 등록 실패 → 에러 메시지 후 다시 입력받거나, 로직에 맞춰 처리
            view.ShowMessage(errorMessage);
            return;
        }

        // 닉네임 등록 성공
        ProceedToLobby("로그인 성공");
    }

    /// <summary>
    /// 로비씬 이동 처리 (별도 ISceneNavigator 등을 써도 좋음)
    /// </summary>
    private void ProceedToLobby(string successMessage)
    {
        view.ShowMessage($"{model.firebaseUser.DisplayName}\n{successMessage}");
        SceneManager.LoadScene((int)SceneType.Lobby);
    }

    private async void HandleRegistration(Func<UniTask<(bool, string)>> registrationFunc, string successMessage)
    {
        view.ShowLoading(true);

        var (result, errorMessage) = await registrationFunc();

        view.ShowLoading(false);

        if (result)
        {
            view.ShowMessage(successMessage);
        }
        else
        {
            view.ShowMessage(errorMessage);
        }
    }

    private void Login_Email(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            view.ShowMessage("email or password Field is NullOrEmpty");
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
            view.ShowMessage("email or password Field is NullOrEmpty");
            return;
        }

        HandleRegistration(() => model.CreateAccountAuthenticateResult(email, password), $"{email} : 회원가입 성공");
    }
}
