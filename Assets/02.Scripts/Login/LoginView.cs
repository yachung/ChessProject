using Michsky.MUIP;
using System;
using TMPro;
using UnityEngine;

public interface ILoginView
{
    event Action<string, string> OnLoginButtonClicked;
    event Action<string, string> OnRegisterButtonClicked;
}

public class LoginView : MonoBehaviour, ILoginView
{
    [SerializeField] private TMP_InputField input_Email;
    [SerializeField] private TMP_InputField input_Password;
    [SerializeField] private ButtonManager btn_Login;
    [SerializeField] private ButtonManager btn_Register;
    [SerializeField] private ButtonManager btn_GoogleLogin;

    public event Action OnGoogleLoginButtonClicked;
    public event Action<string, string> OnLoginButtonClicked;
    public event Action<string, string> OnRegisterButtonClicked;

    public void Start()
    {
        btn_GoogleLogin.onClick.AddListener(() =>
        {
            OnGoogleLoginButtonClicked?.Invoke();
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
}
