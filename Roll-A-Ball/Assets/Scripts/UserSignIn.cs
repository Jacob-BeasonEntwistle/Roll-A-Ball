using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections.Generic;

public class  UserSignIn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Debug.Log("Google Play Sign-In Success");
        }
        else
        {
            // Disable integration with Play Games Services or show a login button to ask users to authenticate
            // Clicking button shuold call PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication)
            Debug.Log("Google Play Sign-In Failed: " + status);
        }
    }
}
