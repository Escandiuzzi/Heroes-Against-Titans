using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseManager : MonoBehaviour {

    Firebase.FirebaseApp app;

    string email;
    string password;

	void Start () {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp, i.e.
                 app = Firebase.FirebaseApp.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }

            // Set up the Editor before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://heroes-agaist-titans.firebaseio.com/");

            // Get the root reference location of the database.
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void ConsultDatabase(Card.ActionStats action)
    {
        FirebaseDatabase.DefaultInstance
      .GetReference(action.GetID().ToString())
      .GetValueAsync().ContinueWith(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
              Debug.Log("Failed to read the database");
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              // Do something with snapshot...

              string name = snapshot.Child("name").Value.ToString();
              float health = float.Parse(snapshot.Child("health").Value.ToString());
              float attack = float.Parse(snapshot.Child("attack").Value.ToString());
              float defense = float.Parse(snapshot.Child("defense").Value.ToString());
              float mana = float.Parse(snapshot.Child("mana").Value.ToString());
              string special = snapshot.Child("special").Value.ToString();

              action.SetName(name);
              action.SetHealthBonus(health);
              action.SetAttack(attack);
              action.SetDefense(defense);
              action.SetMana(mana);
              action.SetSpecial(special);
          }
      });
    }

    public void SetLoginData(string _email, string _password)
    {
        email = _email;
        password = _password;
    }
}
