using Michsky.MUIP;
using System;
using TMPro;
using UnityEngine;
using VContainer;

public interface ILoginView
{
    void ShowMessage(string message);
    void ShowLoading(bool isShow);
    void RequestNicknameInput();

    event Action<string, string> OnLoginButtonClicked;
    event Action<string, string> OnRegisterButtonClicked;
    event Action OnGoogleLoginButtonClicked;
    event Action OnGuestLoginButtonClicked;
    event Action<string> OnNicknameEntered;
}

public class LoginView : MonoBehaviour, ILoginView
{
    [Inject] private readonly UIManager uiManager;

    [SerializeField] private TMP_InputField input_Email;
    [SerializeField] private TMP_InputField input_Password;
    [SerializeField] private TMP_InputField input_Nickname;
    [SerializeField] private ButtonManager btn_Login;
    [SerializeField] private ButtonManager btn_Register;
    [SerializeField] private ButtonManager btn_GoogleLogin;
    [SerializeField] private ButtonManager btn_Guest;

    public event Action<string, string> OnLoginButtonClicked;
    public event Action<string, string> OnRegisterButtonClicked;
    public event Action OnGoogleLoginButtonClicked;
    public event Action OnGuestLoginButtonClicked;
    public event Action<string> OnNicknameEntered;

    public void Start()
    {
        btn_GoogleLogin.onClick.AddListener(() =>
        {
            OnGoogleLoginButtonClicked?.Invoke();
        });

        btn_Guest.onClick.AddListener(() =>
        {
            OnGuestLoginButtonClicked?.Invoke();
        });

        btn_Login.onClick.AddListener(() =>
        {
            OnLoginButtonClicked?.Invoke(input_Email.text, input_Password.text);
        });

        btn_Register.onClick.AddListener(() =>
        {
            OnRegisterButtonClicked?.Invoke(input_Email.text, input_Password.text);
        });
    }

    public void RequestNicknameInput()
    {
        //OnNicknameEntered?.Invoke
    }

    #region UIManager Function

    public void ShowLoading(bool isShow)
    {
        uiManager.ShowLoading(isShow);
    }

    public void ShowMessage(string message)
    {
        uiManager.ShowMessage(message);
    }

    #endregion
}
