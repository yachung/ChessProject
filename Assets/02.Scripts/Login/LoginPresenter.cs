using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoginPresenter : MonoBehaviour
{
    private readonly ILoginView view;
    private readonly LoginModel model;

    private async void LoginAsync(string email, string password)
    {
        await model.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
        });
    }

    private async void RegisterAsync(string email, string password)
    {
        await model.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
        });
    }

    private async void OnLoginButtonClicked()
    {
        view.ShowLoading(true);

        //await LoginAsync(view.Email, view.Password).
            
        //    ContinueWith(result =>
        //{

        //});

        //if (result.User.IsValid())
        //    view.ShowSuccess("Login Success");
        //else
        //    view.ShowError("Login Error");

        //view.ShowLoading(false);
    }

    private async void OnRegisterButtonClicked()
    {
        view.ShowLoading(true);

        //AuthResult result = await RegisterAsync(view.Email, view.Password);

        //if (result.User.IsValid())
        //    view.ShowSuccess("Register Success");
        //else
        //    view.ShowError("Register Error");

        //view.ShowLoading(false);
    }
}
