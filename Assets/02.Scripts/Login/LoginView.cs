using Michsky.MUIP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface ILoginView
{
    string Email { get;}
    string Password { get;}

    void ShowLoading(bool isLoading);
    void ShowError(string message);
    void ShowSuccess(string message);
}

public class LoginView : MonoBehaviour, ILoginView
{
    [SerializeField] private InputField input_Email;
    [SerializeField] private InputField input_Password;
    [SerializeField] private ButtonManager btn_Login;
    [SerializeField] private ButtonManager btn_Register;

    public string Email => input_Email.text;
    public string Password => input_Password.text;

    public void Initialize(Action OnLoginButtonClick, Action OnRegisterButtonClick)
    {
        btn_Login.onClick.AddListener(() => OnLoginButtonClick?.Invoke());
        btn_Register.onClick.AddListener(() => OnRegisterButtonClick?.Invoke());
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
