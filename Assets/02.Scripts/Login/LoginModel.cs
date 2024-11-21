using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class LoginModel
{
    [Inject] private readonly FirebaseManager firebaseManager;

    public string Email;
    public string Password;
    public FirebaseUser firebaseUser;


    private async UniTask<(bool result, string errorMessage)> FirebaseAuthenticateResult(Func<UniTask<(FirebaseUser, string)>> authFunc)
    {
        var (user, errorMessage) = await authFunc();

        if (user != null)
        {
            firebaseUser = user;
            return (true, null);
        }

        return (false, errorMessage);
    }


    public UniTask<(bool result, string errorMessage)> GuestAuthenticateResult()
    {
        return FirebaseAuthenticateResult(() => firebaseManager.SignInAnonymouslyAsync());
    }
    public UniTask<(bool result, string errorMessage)> GoogleAuthenticateResult()
    {
        return FirebaseAuthenticateResult(() => firebaseManager.SignInWithGoogleAsync());
    }
    public UniTask<(bool result, string errorMessage)> EmailAuthenticateResult(string email, string password)
    {
        return FirebaseAuthenticateResult(() => firebaseManager.SignInWithEmailAndPasswordAsync(email, password));
    }
    public UniTask<(bool result, string errorMessage)> CreateAccountAuthenticateResult(string email, string password)
    {
        return FirebaseAuthenticateResult(() => firebaseManager.CreateUserWithEmailAndPasswordAsync(email, password));
    }

    public UniTask<(bool result, string errorMessage)> SetProfileAuthenticateResult(string nickName)
    {
        return FirebaseAuthenticateResult(() => firebaseManager.UpdateUserProfileAsync(firebaseUser, nickName));
    }
}
