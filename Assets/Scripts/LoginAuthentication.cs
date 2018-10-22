using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoginAuthentication : MonoBehaviour {

    [SerializeField]
    InputField emailInput;
    [SerializeField]
    InputField passwordInput;

    [SerializeField]
    GameObject loginCanvas;
    [SerializeField]
    GameObject panel;

    public Text debugText;

    public Color naColor;

    bool newAccount;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set a flag here indiciating that Firebase is ready to use by your
                // application.
                debugText.text += "  Firebase is ready to used";
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                debugText.text += "  Firebase Unity SDK is not safe to use here";
            }
        });

        InitializeFirebase();
        newAccount = false;
    }

    protected void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                //displayName = user.DisplayName ?? "";
                //emailAddress = user.Email ?? "";
                //photoUrl = user.PhotoUrl ?? "";
            }
        }
    }


    public void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (newAccount)
            NewUserAccountLogin(email, password);
        else
            AccountLogin(email, password);
    }
    public void CreateNewAccount()
    {
        newAccount = true;
        panel.GetComponent<Image>().color = naColor;
    }
    void NewUserAccountLogin(string email, string password)
    {
        debugText.text += "  Creating new User";

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                debugText.text += "   CreateUserWithEmailAndPasswordAsync was canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                debugText.text += "   CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception;
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);

            debugText.text += "  Firebase user created successfully.";
            SceneManager.LoadScene("BattleScene");
        });
    }
    void AccountLogin(string email, string password)
    {
        debugText.text += "  Loggin in User";
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                debugText.text += "   SignInWithEmailAndPasswordAsync was canceled. ";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                debugText.text += "   SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception;
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            debugText.text += "User signed in successfully:";
            SceneManager.LoadScene("BattleScene");
        });
    }
}
