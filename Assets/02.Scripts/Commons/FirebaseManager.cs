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
using System.Text.RegularExpressions;

public class FirebaseManager
{
    // 인증을 관리할 객체
    private FirebaseAuth auth;

    private readonly string googleWebAPI = "971240606653-3dualfoth8v7i2dnmgm1r9rgitk6gidp.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;
    private bool isSignin = false;

    public FirebaseUser currentUser;

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

    public async UniTask<(FirebaseUser user, string errorMessage)> SignInAnonymouslyAsync()
    {
        try
        {
            var authResult = await auth.SignInAnonymouslyAsync().AsUniTask();

            currentUser = authResult.User;

            if (string.IsNullOrEmpty(currentUser?.DisplayName))
            {
                await currentUser.UpdateUserProfileAsync(new UserProfile { DisplayName = CreateRandomName()}).AsUniTask();
            }

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

    public async UniTask<(FirebaseUser user, string errorMessage)> UpdateUserProfileAsync(FirebaseUser user, string displayName)
    {
        try
        {
            var task = user.UpdateUserProfileAsync(new UserProfile { DisplayName = displayName }).AsUniTask();
            await task;

            var authResult = await auth.SignInAnonymouslyAsync().AsUniTask();


            if (string.IsNullOrEmpty(user?.DisplayName))
            {
                await user.UpdateUserProfileAsync(new UserProfile { DisplayName = CreateRandomName() }).AsUniTask();
            }

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

    public bool IsValidNickname(string nickname)
    {
        if (string.IsNullOrEmpty(nickname))
            return false;

        if (nickname.Length < 3 || nickname.Length > 12)
            return false;

        // 예: 영문자와 숫자만 허용
        if (!Regex.IsMatch(nickname, "^[a-zA-Z0-9]+$"))
            return false;

        // 금지어 필터링 (필요에 따라 구현)
        // if (ContainsForbiddenWords(nickname))
        //     return false;

        return true;
    }

    private string CreateRandomName()
    {
        return $"Guest{UnityEngine.Random.Range(1000, 10000)}";
    }

    public async UniTask<(FirebaseUser user, string errorMessage)> SignInWithGoogleAsync()
    {
        try
        {
            var googleSignInUser = await GoogleSignIn.DefaultInstance.SignIn().AsUniTask();

            try
            {
                Credential credential = GoogleAuthProvider.GetCredential(googleSignInUser.IdToken, null);
                var firebaseUser = await auth.SignInWithCredentialAsync(credential).AsUniTask();

                currentUser = firebaseUser;

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

    public async UniTask<(FirebaseUser user, string errorMessage)> SignInWithEmailAndPasswordAsync(string email, string password)
    {
        try
        {
            var authResult = await auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            
            currentUser = authResult.User;

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

    public async UniTask<(FirebaseUser user, string errorMessage)> CreateUserWithEmailAndPasswordAsync(string email, string password)
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