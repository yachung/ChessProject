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

    public async UniTask<bool> Login(string email, string password)
    {
        var user = await firebaseManager.LoginAsync(email, password);

        if (user != null)
        {
            return true;
        }

        return false;
    }

    public async UniTask<bool> Register(string email, string password)
    {
        var user = await firebaseManager.RegisterAsync(email, password);

        if (user != null)
        {
            return true;
        }

        return false;
    }
}
