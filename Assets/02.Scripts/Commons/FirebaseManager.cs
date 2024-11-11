using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class FirebaseManager
{
    // 인증을 관리할 객체
    private readonly FirebaseAuth auth;

    public FirebaseManager()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public async UniTask<FirebaseUser> LoginAsync(string email, string password)
    {
        var task = auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();

        var UserCredential = await task;

        if (UserCredential != null) 
        {
            Debug.Log($"로그인 성공 : email : {email}");
            return UserCredential.User;
        }
        else
        {
            Debug.LogError($"로그인 실패 : {UserCredential}");
            return null;
        }
    }

    public async UniTask<FirebaseUser> RegisterAsync(string email, string password)
    {
        // 제공되는 함수 : 이메일과 비밀번호로 회원가입 시켜 줌
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();

        var UserCredential = await task;

        if (UserCredential != null)
        {
            Debug.Log($"회원가입 성공 : email : {email}");
            return UserCredential.User;
        }
        else
        {
            Debug.LogError($"회원가입 실패 : {UserCredential}");
            return null;
        }
    }
}