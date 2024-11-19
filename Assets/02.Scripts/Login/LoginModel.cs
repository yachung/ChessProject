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

    private async UniTask<(bool result, string errorMessage)> FirebaseAuthenticateResult(Func<UniTask<(FirebaseUser, string)>> authFunc)
    {
        var (user, errorMessage) = await authFunc();

        if (user != null)
        {
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

    //public async UniTask<(bool result, string errorMessage)> GetGuestAuthenticate()
    //{
    //    var (user, errorMessage) = await firebaseManager.SignInWithGoogleAsync();

    //    if (user != null)
    //    {
    //        return (true, null);
    //    }

    //    return (false, errorMessage);
    //}

    //public async UniTask<(bool result, string errorMessage)> GetGoogleAuthenticate()
    //{
    //    var(user, errorMessage) = await firebaseManager.SignInWithGoogleAsync();

    //    if (user != null)
    //    {
    //        return (true, null);
    //    }

    //    return (false, errorMessage);
    //}

    //public async UniTask<(bool result, string errorMessage)> GetEmailAuthenticate(string email, string password)
    //{
    //    var (user, errorMessage) = await firebaseManager.SignInWithEmailAndPasswordAsync(email, password);

    //    if (user != null)
    //    {
    //        return (true, null);
    //    }

    //    return (false, errorMessage);
    //}

    //public async UniTask<(bool result, string errorMessage)> GetCreateAccountAuthenticate(string email, string password)
    //{
    //    var (user, errorMessage) = await firebaseManager.CreateUserWithEmailAndPasswordAsync(email, password);

    //    if (user != null)
    //    {
    //        return (true, null);
    //    }

    //    return (false, errorMessage);
    //}
}
