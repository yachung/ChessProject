using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase;
using System;
using Firebase.Extensions;
using Google;
using TMPro;
using Fusion;

public class FirebaseManager
{
    // 인증을 관리할 객체
    private FirebaseAuth auth;

    private readonly string googleWebAPI = "971240606653-3dualfoth8v7i2dnmgm1r9rgitk6gidp.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;
    private bool isSignin = false;

    public FirebaseManager()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Firebase Initialize Failed");
                return;
            }

            Debug.Log("Firebase Initialize Complete");
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;

            // 구글 SDK를 활용한 로그인 기능 등록
            configuration = new GoogleSignInConfiguration { WebClientId = googleWebAPI, RequestEmail = true, RequestIdToken = true };
        });
    }

    public async UniTask<(FirebaseUser user, string errorMessage)> SignInWithGoogleAsync()
    {
        var task = GoogleSignIn.DefaultInstance.SignIn().AsUniTask();

        try
        {
            var googleSignInUser = await task;

            try
            {
                Credential credential = GoogleAuthProvider.GetCredential(googleSignInUser.IdToken, null);
                var firebaseUser = await auth.SignInWithCredentialAsync(credential).AsUniTask();
                return (firebaseUser, null);
            }
            catch (FirebaseException ex)
            {
                Debug.LogError($"FirebaseAuthException: {ex.ErrorCode} - {ex.Message}");
                return (null, $"Firebase 오류: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"FirebaseException: {ex.Message}");
                return (null, $"예기치 못한 오류: {ex.Message}");
            }

        }
        catch (GoogleSignIn.SignInException ex)
        {
            Debug.LogError($"SignInExceptionException: {ex.Message}");
            return (null, $"예기치 못한 오류: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"GoogleSignIn Exception: {ex.Message}");
            return (null, $"예기치 못한 오류: {ex.Message}");
        }
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