using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviour
{
    [SerializeField] InputField emailField;
    [SerializeField] InputField passField;

    // 인증을 관리할 객체
    private FirebaseAuth auth;

    void Awake()
    {
        // 객체 초기화
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login()
    {
        // 제공되는 함수 : 이메일과 비밀번호로 로그인 시켜 줌
        auth.SignInWithEmailAndPasswordAsync(emailField.text, passField.text).ContinueWith(
            task => {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(emailField.text + " 로 로그인 하셨습니다.");
                }
                else
                {
                    Debug.Log("로그인에 실패하셨습니다.");
                }
            }
        );
    }

    public void Register()
    {
        // 제공되는 함수 : 이메일과 비밀번호로 회원가입 시켜 줌
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passField.text).ContinueWith(
            task => {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log(emailField.text + "로 회원가입\n");
                }
                else
                    Debug.Log("회원가입 실패\n");
            }
            );
    }
}