using System;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using Google;

public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase 종속성 확인 및 초기화를 위한 변수
    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    private FirebaseApp _app;
    // Firebase 인증 객체
    private FirebaseAuth _auth;
    // 현재 로그인된 사용자 객체
    private FirebaseUser _user;

    private const string WebClientID = "948952033047-ict8vtg8ddignabevo8tp1t8us0rjlh7.apps.googleusercontent.com";
    //private GoogleSignInConfiguration _configuration;


    void Start()
    {
        // Firebase 종속성을 확인하고 초기화를 진행합니다.
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase가 사용 가능할 때 초기화합니다.
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    // Firebase 초기화 메서드
    void InitializeFirebase()
    {
        _app = FirebaseApp.DefaultInstance;
        _auth = FirebaseAuth.DefaultInstance;
        // 사용자의 로그인 상태가 변경될 때마다 호출될 이벤트를 등록합니다.
        _auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // 사용자의 로그인 상태 변경을 감지하는 메서드
    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;
            if (!signedIn && _user != null)
            {
                Debug.Log("Signed out " + _user.UserId);
            }
            _user = _auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + _user.UserId);
            }
        }
    }

    private void GoogleLogIn()
    {
        // GoogleSignIn.Configuration = new GoogleSignInConfiguration
        // {
        //     RequestIdToken = true,
        //     WebClientId = WebClientID
        // };
        //
        //
        //
        //
        // string googleAccessToken;
        // string googleIdToken;
        //
        // Credential credential =
        //     GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
        // _auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
        //     if (task.IsCanceled) {
        //         Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
        //         return;
        //     }
        //     if (task.IsFaulted) {
        //         Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
        //         return;
        //     }
        //
        //     Firebase.Auth.AuthResult result = task.Result;
        //     Debug.LogFormat("User signed in successfully: {0} ({1})",
        //         result.User.DisplayName, result.User.UserId);
        // });
    }
    public void GuestLogIn()
    {
        _auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });

    }

    void OnDestroy()
    {
        // 객체가 파괴될 때 이벤트 리스너를 제거합니다.
        if (_auth != null)
        {
            _auth.StateChanged -= AuthStateChanged;
            _auth = null;
        }
    }

}
