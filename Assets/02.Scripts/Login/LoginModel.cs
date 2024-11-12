using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class LoginModel
{
    [Inject] private readonly FirebaseManager firebaseManager;

    public string Email;
    public string Password;

    public async UniTask<(bool result, string errorMessage)> AuthenticateUser(string email, string password)
    {
        var (user, errorMessage) = await firebaseManager.LoginAsync(email, password);

        if (user != null)
        {
            return (true, null);
        }

        return (false, errorMessage);
    }

    public async UniTask<(bool result, string errorMessage)> CreateAccount(string email, string password)
    {
        var (user, errorMessage) = await firebaseManager.RegisterAsync(email, password);

        if (user != null)
        {
            return (true, null);
        }

        return (false, errorMessage);
    }
}
