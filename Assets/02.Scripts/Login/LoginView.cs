using Michsky.MUIP;
using System;
using TMPro;
using UnityEngine;

public interface ILoginView
{
    event Action<string, string> OnLoginButtonClicked;
    event Action<string, string> OnRegisterButtonClicked;

    void ShowLoading(bool isLoading);
    void ShowError(string message);
    void ShowSuccess(string message);
}

public class LoginView : MonoBehaviour, ILoginView
{
    [SerializeField] private TMP_InputField input_Email;
    [SerializeField] private TMP_InputField input_Password;
    [SerializeField] private ButtonManager btn_Login;
    [SerializeField] private ButtonManager btn_Register;

    public event Action<string, string> OnLoginButtonClicked;
    public event Action<string, string> OnRegisterButtonClicked;

    public void Start()
    {
        btn_Login.onClick.AddListener(() =>
        {
            OnLoginButtonClicked?.Invoke(input_Email.text, input_Password.text);
        });

        btn_Register.onClick.AddListener(() =>
        {
            OnRegisterButtonClicked?.Invoke(input_Email.text, input_Password.text);
        });
    }

    public void ShowError(string message)
    {
        Debug.Log(message);
    }

    public void ShowLoading(bool isLoading)
    {
        Debug.Log($"isLoading : {isLoading}");
    }

    public void ShowSuccess(string message)
    {
        Debug.Log(message);
    }
}
