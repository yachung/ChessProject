using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase;
using System;

public class FirebaseManager
{
    // 인증을 관리할 객체
    private readonly FirebaseAuth auth;

    public FirebaseManager()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public async UniTask<(FirebaseUser user, string errorMessage)> LoginAsync(string email, string password)
    {
        try
        {
            var authResult = await auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();

            return (authResult.User, null);
        }
        catch (FirebaseException ex)
        {
            Debug.LogError($"FirebaseAuthException: {ex.ErrorCode} - {ex.Message}");
            return (null, $"Firebase 오류: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
            return (null, $"예기치 못한 오류: {ex.Message}");
        }
    }

    public async UniTask<(FirebaseUser user, string errorMessage)> RegisterAsync(string email, string password)
    {
        try
        {
            // 제공되는 함수 : 이메일과 비밀번호로 회원가입 시켜 줌
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();

            return (authResult.User, null);
        }
        catch (FirebaseException ex)
        {
            Debug.LogError($"FirebaseAuthException: {ex.ErrorCode} - {ex.Message}");
            return (null, $"Firebase 오류: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
            return (null, $"예기치 못한 오류: {ex.Message}");
        }
    }
}