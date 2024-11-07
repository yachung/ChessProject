using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginModel
{
    public FirebaseAuth auth;

    public LoginModel()
    {
        auth = FirebaseAuth.DefaultInstance;
    }
}
